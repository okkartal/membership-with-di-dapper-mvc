using System;
using System.Collections.Generic;
using System.Data;
using Infrastructure.Dapper;

namespace Infrastructure
{
    public static class DapperExtensions
    {
        private static void OpenConnection(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        private static void CloseConnection(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        private static TRes ConnectionAction<TRes>(this IDbConnection connection, Func<TRes> action)
        {
            connection.OpenConnection();
            var res = action();
            connection.CloseConnection();
            return res;
        }

        private static IEnumerable<TRes> ExecuteInside<TRes>(this IDbConnection connection, string command, dynamic parameters, CommandType commandType)
        {
            return connection.ConnectionAction(() => connection.Query<TRes>(command, (object)parameters, null, false, null, commandType));
        }

        public static IEnumerable<TRes> ExecStoredProcedure<TRes>(this IDbConnection connection, string procedureName, dynamic parameters)
        {
            return connection.ExecuteInside<TRes>(procedureName, (object)parameters, CommandType.StoredProcedure);
        }

        public static IEnumerable<TRes> ExecSql<TRes>(this IDbConnection connection, string sqlCommand, dynamic parameters)
        {
            return connection.ExecuteInside<TRes>(sqlCommand, (object)parameters, CommandType.Text);
        }



        private static int ExecNonQueryInside(this IDbConnection connection, string sqlCommand, dynamic parameters, CommandType commandType)
        {
            return connection.Execute(sqlCommand, (object)parameters, null, null, commandType);
        }
        public static int ExecNonQueryStoredProc(this IDbConnection connection, string sqlCommand, dynamic parameters)
        {
            return connection.ExecNonQueryInside(sqlCommand, (object)parameters, CommandType.StoredProcedure);
        }
        public static int ExecNonQuerySql(this IDbConnection connection, string sqlCommand, dynamic parameters)
        {
            return connection.ExecNonQueryInside(sqlCommand, (object)parameters, CommandType.Text);
        }
    }
}
