using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SqlRepository.BussinessRepository
{
    public class PropietarioRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public PropietarioRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int AddPropietario(Propietario propietario)
        {
            string procName = "spDoctorInsert";
            var param = new DynamicParameters();
            int doctorId = 0;

            param.Add("@Id", propietario.Id, null, ParameterDirection.Output);
            param.Add("@FirstName", propietario.Nombres);
            param.Add("@LastName", propietario.Tipodoc);
            param.Add("@Phone", propietario.NumDoc);

            try
            {
                SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);

                doctorId = param.Get<int>("@Id");
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return doctorId;
        }

        public bool DeleteDoctor(int propietarioId)
        {
            bool IsDeleted = true;
            var SqlQuery = @"DELETE FROM Doctors WHERE DoctorID = @Id";

            using (IDbConnection conn = _connectionFactory.GetConnection)
            {
                var rowsaffected = conn.Execute(SqlQuery, new { Id = propietarioId });
                if (rowsaffected <= 0)
                {
                    IsDeleted = false;
                }
            }
            return IsDeleted;
        }

        public Propietario GetDoctorById(int propietarioId)
        {
            var Doctor = new Propietario();
            var procName = "spDoctorFetch";
            var param = new DynamicParameters();
            param.Add("@DoctorId", propietarioId);

            try
            {

                using (IDbConnection conn = _connectionFactory.GetConnection)
                {
                    var result = conn.Query<Propietario>(procName, param: param, commandType: CommandType.StoredProcedure);
                    Doctor = result.FirstOrDefault();
                }


            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return Doctor;
        }

        public IList<Propietario> GetDoctorsByQuery()
        {
            var EmpList = new List<Propietario>();
            var SqlQuery = @"SELECT [Id]
                            ,[FirstName]
                            ,[LastName]
                            ,[Phone]
                            ,[Email]
                            ,[Address]
                            ,[CreatedDate]
                        FROM [DoctorData].[dbo].[Doctors]";

            using (IDbConnection conn = _connectionFactory.GetConnection)
            {
                var result = conn.Query<Propietario>(SqlQuery);
                return result.ToList();
            }
        }

        public bool UpdateDoctor(int DoctorId, Propietario propietario)
        {
            string procName = "spDoctorUpdate";
            var param = new DynamicParameters();
            bool IsSuccess = true;


            param.Add("@DoctorId", propietario.Id);
            param.Add("@FirstName", propietario.Nombres);
            param.Add("@LastName", propietario.NumDoc);;
            param.Add("@Phone", propietario.Tipodoc);

            try
            {
                var rowsAffected = SqlMapper.Execute(_connectionFactory.GetConnection,
                procName, param, commandType: CommandType.StoredProcedure);
                if (rowsAffected <= 0)
                {
                    IsSuccess = false;
                }
            }
            finally
            {
                _connectionFactory.CloseConnection();
            }

            return IsSuccess;
        }
    }
}
