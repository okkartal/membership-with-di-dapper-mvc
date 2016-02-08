using Business.Interface;
using Entity;
using Infrastructure.Constraints.Constant;
using Infrastructure.Security;
using Infrastructure.ServiceResult;
using ViewModel;
using DataContract;
using System;
using System.Collections.Generic;
using Infrastructure.Validation;

namespace Business.Impl
{
    public class MemberBusiness : IMemberBusiness
    {
        private readonly IHashProvider _hashProvider;
        private readonly IMemberRepository _memberRepository;
        public MemberBusiness(IHashProvider hashProvider, IMemberRepository memberRepository)
        {
            _hashProvider = hashProvider;
            _memberRepository = memberRepository;
        }

        public ResultSet<Member> AddMember(MemberRegisterViewModel member)
        {
            var result = new ResultSet<Member>();

            /******This nickname is exists*******************/
            if (_memberRepository.GetMemberByNickname(member.NickName) != null)
            {
                result.Message = "This nickname is already in use";
                return result;
            }
             

            if (!string.IsNullOrEmpty(member.Password) && member.Password.Length > 16)
            {
                result.Message = "Invalid password string length";
                return result;
            }

             
            if (!string.IsNullOrEmpty(member.Password))
            {
                string hash, salt;
                _hashProvider.GetHashAndSaltString(member.Password, out hash, out salt);
                member.PasswordHash = hash;
                member.PasswordSalt = salt;
            }
            var newMember = _memberRepository.AddMember(member);

            if (newMember != null)
            {


                result.Success = true;
                result.Message = Messages.OperationSuccess;
                result.Object = newMember;
            }

            return result;
        }

        public ResultSet<Member> GetMemberByMemberId(int memberId)
        {
            var result = new ResultSet<Member>();
            var member = _memberRepository.GetMemberByMemberId(memberId);
            {
                if (member != null)
                {
                    result.Message = Messages.OperationSuccess;
                    result.Success = true;
                    result.Object = member;
                }
            }
            return result;
        }

        public ResultSet<MemberDetailViewModel> GetMemberDetails(int memberId)
        {
            var result = new ResultSet<MemberDetailViewModel>();
            var tempMemberDetail = _memberRepository.GetMemberDetails(memberId);
            if (tempMemberDetail != null)
            {
                result.Message =Messages.OperationSuccess;
                result.Success = true;
                result.Object = tempMemberDetail;
            }
            return result;
        }

        public ResultSet<Member> MemberLogin(MemberLogin member)
        {
            var result = new ResultSet<Member>();
             
             
            Member loginMember = _memberRepository.MemberLogin(new Member() { Email = member.NickName });
            if (loginMember == null)
            {
                result.Message = "Member not found";
                return result;
            }
             

            if (string.IsNullOrEmpty(loginMember.PasswordHash) || !_hashProvider.VerifyHashString(member.Password, loginMember.PasswordHash, loginMember.PasswordSalt))
            {
                result.Message = "Invalid password";
                return result;
            }
            result.Object = loginMember;
            result.Message = "Operation success";
            result.Success = true;

            return result;
        }


        public ResultSet<MemberDetailViewModel> UpdateMemberDetail(MemberDetailViewModel memberDetail)
        {
            var result = new ResultSet<MemberDetailViewModel>();
            result.Object = memberDetail;
            var member = GetMemberByMemberId(memberDetail.MemberId);
            if (member.Success)
            {
                DateTime dateTime;
                #region Basic Fields Validations

                if (string.IsNullOrEmpty(memberDetail.NickName) || !ValidationUtils.UserNameIsValid(memberDetail.NickName))
                {
                    result.Message = "Invalid nickname"; 
                    return result;
                }

                else if ((!string.IsNullOrEmpty(memberDetail.CurrentPassword) && memberDetail.CurrentPassword.Length > 16))
                {
                    result.Message = "Invalid password(maximum current password length can be 16 character)"; 
                    return result;
                }

                else if ((!string.IsNullOrEmpty(memberDetail.NewPassword) && memberDetail.NewPassword.Length > 16))
                {
                    result.Message = "Invalid password(maximum new password length can be 16 character)";
                    return result;
                }

                else if ((!string.IsNullOrEmpty(memberDetail.NewPasswordMatch) && memberDetail.NewPasswordMatch.Length > 16))
                {
                    result.Message = "Invalid password(maximum password repeat length can be 16 character)";
                }
 
                #endregion

                #region This fields are exists?
                /*************This nickname is exists***************************************/
                var _existMember = _memberRepository.GetMemberByNickname(memberDetail.NickName);
                if (_existMember != null && _existMember.Id != member.Object.Id)
                {
                    result.Message = "This nick name is in use"; 
                    return result;
                }

                

                #endregion

                
 

                #region Password Operations
                else if (!string.IsNullOrEmpty(memberDetail.NewPassword) && string.IsNullOrEmpty(memberDetail.NewPasswordMatch))
                {
                    result.Message = "New password and repeat did not match"; 
                    return result;
                }

                else if ((!string.IsNullOrEmpty(memberDetail.NewPassword) || !string.IsNullOrEmpty(memberDetail.NewPasswordMatch)) && (!string.IsNullOrEmpty(member.Object.PasswordHash))
                    && string.IsNullOrEmpty(memberDetail.CurrentPassword))
                {
                    result.Message = "Please enter current password"; 
                    return result;
                }

                else if (!string.IsNullOrEmpty(memberDetail.NewPassword) && !string.IsNullOrEmpty(memberDetail.NewPasswordMatch))
                {
                    if (memberDetail.NewPassword != memberDetail.NewPasswordMatch)
                    {
                        result.Message  = "New password and repeat did not match";
                        return result;
                    }

                    string _hash = string.Empty, _salt = string.Empty;

                    if (!string.IsNullOrEmpty(member.Object.PasswordHash))
                        if (!_hashProvider.VerifyHashString(memberDetail.CurrentPassword, member.Object.PasswordHash, member.Object.PasswordSalt))
                        {
                            result.Message = "Current password is wrong"; 
                            return result;
                        }


                    _hashProvider.GetHashAndSaltString(memberDetail.NewPassword, out _hash, out _salt);
                    memberDetail.PasswordHash = _hash;
                    memberDetail.PasswordSalt = _salt;
                }
                #endregion

               

                
                var tempMemberDetail = _memberRepository.UpdateMemberDetail(memberDetail);

                if (tempMemberDetail != null)
                {
                    result.Success = true;
                    result.Object = tempMemberDetail;
                    result.Message = "Your informations are updated";
                }
            }
            return result;
        }
    }
}
