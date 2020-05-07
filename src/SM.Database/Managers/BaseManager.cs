
using Microsoft.Isam.Esent.Interop;
using System;
using System.Text;
using Weiss.Data.Mapper;

namespace SM.Managers
{
    public class BaseManager : IDisposable {
        private ObjectDataMapper _mapper;

        public ObjectDataMapper Mapper
        {
            get {
                if(_mapper == null)
                {
                    _mapper = ObjectDataMapper.Init(Config.Current.ConnectionString);
                }
                return _mapper;
            }
        }

        public BaseManager(BaseManager bm = null)
        {
            if (bm != null)
                this._mapper = bm.Mapper;
        }

        public void Commit() => _mapper.Commit();

        public void Rollback() => _mapper.Rollback();

        public void Dispose()
        {
            _mapper?.Commit();
            _mapper?.Close();
            _mapper?.Dispose();
            _mapper = null;

            GC.Collect();
        }
    }
}
