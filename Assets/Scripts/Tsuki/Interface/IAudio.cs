// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/08 19:02
// @version: 1.0
// @description:
// *****************************************************************************

namespace Tsuki.Interface
{
    public interface IAudio
    {
        public void PlaySfx(string name);
        public void PlayBgm(string name, bool fadeOut = true);
    }
}
