using MASA.Framework.Exceptions.Localization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MASA.Framework.Exceptions;

namespace System
{
    public class UserFriendlyException : Exception
    {
        private readonly Enum _localizeEnum;
        private static readonly ConcurrentDictionary<LocalizeDataCacheKey, Lazy<ILocalizeData>> _cache = new();

        public UserFriendlyException(Enum localizeEnum)
        {
            _localizeEnum = localizeEnum;
        }

        public UserFriendlyException(string message)
            : base(message)
        {

        }

        public ILocalizeData LocalizeData
        {
            get
            {
                if (_localizeEnum == null)
                {
                    return default;
                }

                var lazyLocalizeData = _cache.GetOrAdd(new LocalizeDataCacheKey(_localizeEnum.GetType(), _localizeEnum.ToString()), r => new Lazy<ILocalizeData>(CreateLocalizeData, isThreadSafe: true));
                return lazyLocalizeData.Value;
            }
        }

        private ILocalizeData CreateLocalizeData()
        {
            return _localizeEnum.GetType().GetField(_localizeEnum.ToString()).GetCustomAttributes(false).OfType<LocalizeDescriptionAttribute>().FirstOrDefault();
        }
    }
}
