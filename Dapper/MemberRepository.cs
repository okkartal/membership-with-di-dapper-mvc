using System.Linq;
using DataContract;
using Entity;
using Infrastructure;
using ViewModel;

namespace Dapper
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DbConnectionFactory _connection;
        public MemberRepository(DbConnectionFactory connection)
        {
            _connection = connection;
        }

      

        public Member GetMemberByMemberId(int memberId)
        {
            return _connection.DbPath.ExecStoredProcedure<Member>("GetMemberByMemberId", new { MemberId = memberId }).FirstOrDefault();
        }

        public Member GetMemberByNickname(string nickName)
        {
            return _connection.DbPath.ExecStoredProcedure<Member>("GetMemberByNickName", new { nickName }).FirstOrDefault();
        }

        public Member MemberLogin(Member member)
        {
           return _connection.DbPath.ExecStoredProcedure<Member>("GetMemberLogin", new { member.Email }).FirstOrDefault();
       

        }


        public Member AddMember(MemberRegisterViewModel member)
        {
            //Member newMember = _connection.DbPath.ExecStoredProcedure<Member>("AddMember",
            //new
            //{
            //    member.NickName,
            //    member.PasswordHash,
            //    member.PasswordSalt 
               
            //}).FirstOrDefault();
            //return newMember;
            //FAKE DATA
            return new Member() {NickName = member.NickName};
        }


        public MemberDetailViewModel GetMemberDetails(int memberId)
        {
            return _connection.DbPath.ExecStoredProcedure<MemberDetailViewModel>("GetMemberDetail",
                new
                {
                    MemberId = memberId
                }).FirstOrDefault();
        }



        public MemberDetailViewModel UpdateMemberDetail(MemberDetailViewModel memberDetail)
        {

            return _connection.DbPath.ExecStoredProcedure<MemberDetailViewModel>("UpdateMemberDetail",
                new
                {
                    memberDetail.MemberId,
                    memberDetail.PasswordHash,
                    memberDetail.PasswordSalt,
                    memberDetail.NickName
                }).FirstOrDefault();
        }
    }
}
