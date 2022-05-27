using Structr.Notices;
using System;

namespace Structr.Tests.Notices.TestUtils
{
    internal class CustomNoticeHandler : NoticeHandler<CustomNotice>
    {
        protected override void Handle(CustomNotice notice)
        {
            throw new NotImplementedException(notice.Title);
        }
    }
}
