using FreshMvvm;

namespace Quaestor.Forms.Core
{
    public abstract class BaseViewModel : FreshBasePageModel
    {
        #region Properties

        public abstract string Title { get; }

        #endregion

        #region FreshBasePageModel

        public override void Init(object initData)
        {
            Init();
        }

        #endregion

        #region Public

        public virtual void Init()
        {
        }

        #endregion
    }
}