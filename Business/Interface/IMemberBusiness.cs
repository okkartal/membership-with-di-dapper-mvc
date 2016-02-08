using Entity;
using Infrastructure.ServiceResult;
using ViewModel;

namespace Business.Interface
{
    public interface IMemberBusiness
    {

        ResultSet<Member> MemberLogin(MemberLogin member);

        ResultSet<Member> GetMemberByMemberId(int memberId);

        ResultSet<MemberDetailViewModel> GetMemberDetails(int memberId);

        ResultSet<MemberDetailViewModel> UpdateMemberDetail(MemberDetailViewModel memberDetail);

        ResultSet<Member> AddMember(MemberRegisterViewModel member);
    }
}
