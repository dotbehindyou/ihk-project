using System;
using System.Collections.Generic;
using System.Text;
using Weiss.Data.Mapper;

namespace SM.API.Managers
{
    public class BaseManager
    {
        private static ObjectDataMapper _mapper = ObjectDataMapper.Init(Config.ConnectionString);

        protected ObjectDataMapper Mapper
        {
            get
            {
                String msg;
                if (!_mapper.TestConnection(out msg))
                    _mapper = ObjectDataMapper.Init(Config.ConnectionString);
                return _mapper;
            }
        }


    }
}
