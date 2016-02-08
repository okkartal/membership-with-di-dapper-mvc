using Entity;
using ViewModel;

namespace DataContract
{
    public interface IMemberRepository
    {
        Member GetMemberByMemberId(int memberId);
        Member GetMemberByNickname(string nickName);
        Member MemberLogin(Member member);
        Member AddMember(MemberRegisterViewModel member);
        MemberDetailViewModel GetMemberDetails(int memberId);

        MemberDetailViewModel UpdateMemberDetail(MemberDetailViewModel memberDetail);
    }
}
