using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Weiss.Data.Mapper;

namespace SM.API.Managers
{
    public class BaseManager : IDisposable
    {
        private readonly String _connectionString;

        private ObjectDataMapper _mapper;

        protected ObjectDataMapper Mapper
        {
            get
            {
                String msg;
                if (!_mapper.TestConnection(out msg))
                    _mapper = ObjectDataMapper.Init(_connectionString);
                return _mapper;
            }
        }

        public BaseManager(String connectionString)
        {
            this._connectionString = connectionString;
            _mapper = ObjectDataMapper.Init(this._connectionString);
        }

        public void Dispose()
        {
            Mapper.Close();
        }
    }
}
