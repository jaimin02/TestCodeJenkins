using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Utility;
using System.Collections;
using SS.Web;

namespace DatabaseOperation.DBContract
{
    public class SqlHelper
    {
        string ConnectionString = string.Empty;
        static SqlConnection con;
        //string ConnectionStringBiz = string.Empty;
        //static SqlConnection conBiz;

        private SqlConnection objConnection;
        //private SqlConnection objConnectionBiz;
        private SqlCommand objCommand;
        private SqlTransaction objTransaction;
        //private SqlTransaction objTransactionBiz;
        private SqlDataAdapter objDataAdapter;
        private SqlDataReader objReader;
        private Boolean _InnerCall;
        //private Boolean _InnerCallBiz;


        public SqlHelper()
        {
            //ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            //con = new SqlConnection(ConnectionString);

            ConnectionString = ServerHelper.DefaultDecryption(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);
            con = new SqlConnection(ConnectionString);

            //ConnectionStringBiz = ConfigurationManager.ConnectionStrings["connectionBiz"].ConnectionString;
            //conBiz = new SqlConnection(ConnectionStringBiz);
        }

        internal System.Data.SqlClient.SqlTransaction Transaction
        {
            get { return objTransaction; }
        }

        private void InOpenConnection(Boolean InnerCall_1)
        {
            if (objConnection == null)
            {
                objConnection = new SqlConnection();
                //objConnection.ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString; ;
                objConnection.ConnectionString = SS.Web.ServerHelper.DefaultDecryption(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);
                objConnection.Open();
                _InnerCall = InnerCall_1;
            }
        }
        //private void InOpenConnectionBiz(Boolean InnerCall_1)
        //{
        //    if (objConnectionBiz == null)
        //    {
        //        objConnectionBiz = new SqlConnection();
        //        objConnectionBiz.ConnectionString = ConfigurationManager.ConnectionStrings["connectionBiz"].ConnectionString; ;
        //        objConnectionBiz.Open();
        //        _InnerCallBiz = InnerCall_1;
        //    }
        //}

        private void InCloseConnection(Boolean InnerCall_1)
        {
            if (InnerCall_1 & (!_InnerCall))
            {
                return;
            }
            objConnection.Close();
            objConnection.Dispose();
            objConnection = null;
        }
        //private void InCloseConnectionBiz(Boolean InnerCall_1)
        //{
        //    if (InnerCall_1 & (!_InnerCallBiz))
        //    {
        //        return;
        //    }
        //    objConnectionBiz.Close();
        //    objConnectionBiz.Dispose();
        //    objConnectionBiz = null;
        //}

        public DataSet ExecuteQueryWithCmd(string query)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();

            if (con == null)
            {
                SetConnection();
            }
            cmd.Connection = con;
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(ds);
            return ds;

        }

        public void SetConnection()
        {
            if (ConnectionString == string.Empty)
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            }
            con = new SqlConnection(ConnectionString);
        }
        //public void SetConnectionBiz()
        //{
        //    if (ConnectionStringBiz == string.Empty)
        //    {
        //        ConnectionStringBiz = ConfigurationManager.ConnectionStrings["connectionBiz"].ConnectionString;
        //    }
        //    conBiz = new SqlConnection(ConnectionStringBiz);
        //}

        public void Open_Connection()
        {
            InOpenConnection(false);
        }
        //public void Open_ConnectionBiz()
        //{
        //    InOpenConnectionBiz(false);
        //}

        public void Close_Connection()
        {
            InCloseConnection(false);
        }
        //public void Close_ConnectionBiz()
        //{
        //    InCloseConnectionBiz(false);
        //}

        public void Begin_Transaction()
        {
            InOpenConnection(true);

            if (objTransaction == null)
            {
                objTransaction = objConnection.BeginTransaction();
            }

        }
        //public void Begin_TransactionBiz()
        //{
        //    InOpenConnectionBiz(true);

        //    if (objTransactionBiz == null)
        //    {
        //        objTransactionBiz = objConnectionBiz.BeginTransaction();
        //    }

        //}

        public void Commit_Transaction()
        {
            if ((objTransaction != null))
            {
                objTransaction.Commit();
                objTransaction = null;
            }
            InCloseConnection(true);
        }
        //public void Commit_TransactionBiz()
        //{
        //    if ((objTransactionBiz != null))
        //    {
        //        objTransactionBiz.Commit();
        //        objTransactionBiz = null;
        //    }
        //    InCloseConnectionBiz(true);
        //}

        public void RollBack_Transaction()
        {
            if ((objTransaction != null))
            {
                objTransaction.Rollback();
                objTransaction = null;
            }
            InCloseConnection(true);

        }
        //public void RollBack_TransactionBiz()
        //{
        //    if ((objTransactionBiz != null))
        //    {
        //        objTransactionBiz.Rollback();
        //        objTransactionBiz = null;
        //    }
        //    InCloseConnectionBiz(true);

        //}

        #region Data Retrival Function

        public DataTable GetDataTable(string TblName_1, string WhereCondition_1, Utilities.DataRetrievalModeEnum DataRetrival_1)
        {
            DataTable Sql_DtTbl_Retu = new DataTable();
            var SqlAdp = new SqlDataAdapter();
            var Sql_Cmd = new SqlCommand();
            string qStr = "";

            TblName_1 = TblName_1.ToUpper();
            try
            {
                if (string.IsNullOrEmpty(TblName_1))
                {
                    throw new Exception("Invalid Table Ref.");
                    return Sql_DtTbl_Retu;
                }

                if (DataRetrival_1 == Utilities.DataRetrievalModeEnum.DatatTable_Query)
                {
                }
                else
                {
                    qStr = "Select * From " + TblName_1;
                    if (DataRetrival_1 == Utilities.DataRetrievalModeEnum.DataTable_Empty)
                    {
                        WhereCondition_1 = " Where 1 = 2 ";
                    }

                    else if (DataRetrival_1 == Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition)
                    {
                        if (Convert.ToString(WhereCondition_1).Trim() == "")
                        {
                            throw new Exception("Invalid or Blank Where Condtion");
                            return Sql_DtTbl_Retu;
                        }
                        WhereCondition_1 = " Where " + WhereCondition_1;
                    }

                    else if (DataRetrival_1 == Utilities.DataRetrievalModeEnum.DataTable_AllRecords)
                    {
                        WhereCondition_1 = " Where 1 = 1 ";

                    }
                    qStr = qStr + WhereCondition_1;
                }

                InOpenConnection(true);
                Sql_Cmd = new SqlCommand(qStr, this.objConnection, this.objTransaction);
                Sql_Cmd.CommandTimeout = 10000;
                SqlAdp = new SqlDataAdapter(Sql_Cmd);
                //Sql_DtTbl_Retu = new DataTable();
                SqlAdp.Fill(Sql_DtTbl_Retu);
                Sql_DtTbl_Retu.TableName = TblName_1;

            }
            catch (Exception ex)
            {
                throw ex;
                return Sql_DtTbl_Retu;
            }
            finally
            {
                InCloseConnection(true);
            }
            return Sql_DtTbl_Retu;

        }

        public DataTable GetDataTable(string TblName_1, Utilities.DataRetrievalModeEnum DataRetrival_1)
        {
            DataTable Sql_DtTbl_Retu = new DataTable();
            var SqlAdp = new SqlDataAdapter();
            var Sql_Cmd = new SqlCommand();
            string qStr = "";

            TblName_1 = TblName_1.ToUpper();
            try
            {
                if (string.IsNullOrEmpty(TblName_1))
                {
                    throw new Exception("Invalid Table Ref.");
                    return Sql_DtTbl_Retu;
                }
                qStr = "Select * From " + TblName_1;


                InOpenConnection(true);
                Sql_Cmd = new SqlCommand(qStr, this.objConnection, this.objTransaction);
                Sql_Cmd.CommandTimeout = 10000;
                SqlAdp = new SqlDataAdapter(Sql_Cmd);
                //Sql_DtTbl_Retu = new DataTable();
                SqlAdp.Fill(Sql_DtTbl_Retu);
                Sql_DtTbl_Retu.TableName = TblName_1;

            }
            catch (Exception ex)
            {
                throw ex;
                return Sql_DtTbl_Retu;
            }
            finally
            {
                InCloseConnection(true);
            }
            return Sql_DtTbl_Retu;

        }

        public DataSet ExecuteQuery_DataSet(string strQuery)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                this.InOpenConnection(true);
                objCommand = new SqlCommand();
                objCommand.Connection = objConnection;
                objCommand.CommandType = CommandType.Text;
                objCommand.CommandText = strQuery;

                objDataAdapter = new SqlDataAdapter();
                objDataAdapter.SelectCommand = objCommand;

                if ((this.objTransaction != null))
                {
                    objCommand.Transaction = objTransaction;
                }

                if (dsReturn.Tables.Count != 0)
                {
                    dsReturn.Tables.Clear();
                }
                objDataAdapter.Fill(dsReturn);
                return dsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.InCloseConnection(true);
            }

        }

        //public DataSet ExecuteQuery_DataSetBiz(string strQuery)
        //{
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        this.InOpenConnectionBiz(true);
        //        objCommand = new SqlCommand();
        //        objCommand.Connection = objConnectionBiz;
        //        objCommand.CommandType = CommandType.Text;
        //        objCommand.CommandText = strQuery;

        //        objDataAdapter = new SqlDataAdapter();
        //        objDataAdapter.SelectCommand = objCommand;

        //        if ((this.objTransactionBiz != null))
        //        {
        //            objCommand.Transaction = objTransactionBiz;
        //        }

        //        if (dsReturn.Tables.Count != 0)
        //        {
        //            dsReturn.Tables.Clear();
        //        }
        //        objDataAdapter.Fill(dsReturn);
        //        return dsReturn;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        this.InCloseConnectionBiz(true);
        //    }

        //}

        public object ExecuteQuery_Scalar(string strQuery)
        {
            object functionReturnValue = null;

            object objscalar = null;

            try
            {
                InOpenConnection(true);

                objCommand = new SqlCommand(strQuery, objConnection, objTransaction);
                objscalar = objCommand.ExecuteScalar();
                InCloseConnection(true);

            }
            catch (Exception ex)
            {
                InCloseConnection(true);
                throw ex;
                return functionReturnValue;
            }

            return objscalar;
            return functionReturnValue;

        }

        public DataSet ExecuteSP_DataSet(string strProcedure, ArrayList colParam)
        {
            DataSet functionReturnValue = default(DataSet);
            DataSet objDataSet = new DataSet();
            object param = null;

            try
            {
                objDataAdapter = new SqlDataAdapter();

                this.InOpenConnection(true);

                objCommand = new SqlCommand(strProcedure, objConnection);
                objCommand.CommandType = CommandType.StoredProcedure;

                if ((this.objTransaction != null))
                {
                    objCommand.Transaction = objTransaction;
                }

                foreach (object param_loopVariable in colParam)
                {
                    param = param_loopVariable;
                    objCommand.Parameters.Add(param);

                }

                objDataAdapter.SelectCommand = objCommand;
                objDataAdapter.Fill(objDataSet);
                return objDataSet;

            }
            catch (Exception ex)
            {
                throw ex;
                return functionReturnValue;
            }
            finally
            {
                this.InCloseConnection(true);
            }
            return functionReturnValue;

        }

        public DataSet ExecuteProcudereWithParams(string SPName, Hashtable parms)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                this.InOpenConnection(true);
                objCommand = new SqlCommand();
                objDataAdapter = new SqlDataAdapter();
                objCommand.Connection = objConnection;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Transaction = objTransaction;
                objCommand.CommandText = SPName;
                if (parms.Count > 0)
                {
                    foreach (DictionaryEntry de in parms)
                    {
                        objCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }
                }
                objDataAdapter.SelectCommand = objCommand;
                objDataAdapter.Fill(dsReturn);
                return dsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.InCloseConnection(true);
            }
        }

        public bool GetFromDB(string SPName, Hashtable parms, ref ArrayList ProcedureOutPutVal_Retu, ref DataTable dtReturn, ref string eStr_Retu, bool SetTimeOut = false)
        {
            DataTable tbl_ProcDtl = null;
            System.Data.SqlClient.SqlParameter SqlPara = null;
            ParameterReturnValue ParaRetuVal = null;
            ArrayList ParaRowArr = null;
            string Proc_ColName = "";
            int ColIndex_1 = -1;

            try
            {
                if (!getSqlObjectInfo(SPName, ref tbl_ProcDtl, ref eStr_Retu))
                {
                    eStr_Retu = "Error Occured While Retrieving Procedure Info.";
                    return false;
                }

                this.InOpenConnection(true);
                objCommand = new SqlCommand();
                objDataAdapter = new SqlDataAdapter();
                objCommand.Connection = objConnection;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Transaction = objTransaction;
                objCommand.CommandText = SPName;
                ParaRowArr = new ArrayList();
                if (parms.Count > 0)
                {
                    foreach (DataRow dr_P in tbl_ProcDtl.Rows)
                    {
                        Proc_ColName = dr_P["COLNAME"].ToString();
                        ColIndex_1 = Convert.ToInt32(dr_P["COLINDEX"]) - 1;

                        SqlPara = new SqlParameter();
                        SqlPara.ParameterName = dr_P["COLNAME"].ToString();
                        SqlPara.Direction = ParameterDirection.Input;

                        if (Convert.ToInt32(dr_P["ISOUTPUT"]) == 0)
                        {
                            SqlPara.Value = parms[Proc_ColName];
                        }
                        else if (Convert.ToInt32(dr_P["ISOUTPUT"]) == 1)
                        {
                            SqlPara.Direction = ParameterDirection.Output;
                            SqlPara.Size = Convert.ToInt32(dr_P["LENGTH"]);

                            ParaRetuVal = new ParameterReturnValue();
                            ParaRetuVal.ParameterName = Convert.ToString(dr_P["COLNAME"]);
                            ParaRetuVal.ParameterIndex = ColIndex_1;
                            ParaRowArr.Add(ParaRetuVal);
                        }
                        objCommand.Parameters.Add(SqlPara);
                    }
                }

                if (SetTimeOut)
                {
                    objCommand.CommandTimeout = 180000;
                }
                objDataAdapter.SelectCommand = objCommand;
                objDataAdapter.Fill(dtReturn);

                foreach (ParameterReturnValue retun_val in ParaRowArr)
                {
                    ProcedureOutPutVal_Retu.Add((Convert.IsDBNull(objCommand.Parameters[retun_val.ParameterName].Value) ? "" : objCommand.Parameters[retun_val.ParameterName].Value));
                }

                return true;
            }
            catch (Exception ex)
            {
                ProcedureOutPutVal_Retu = null;
                eStr_Retu = ex.Message;
                dtReturn = null;
                return false;
            }
            finally
            {
                this.InCloseConnection(true);
            }
        }

        //public DataSet ExecuteProcedureWithParams(string SPName, DataTable dt, Hashtable parms)
        //{
        //    DataSet dsReturn = new DataSet();
        //    Hashtable ht = new Hashtable();
        //    try
        //    {
        //        this.InOpenConnection(true);
        //        objCommand = new SqlCommand();
        //        objDataAdapter = new SqlDataAdapter();
        //        objCommand.Connection = objConnection;
        //        objCommand.CommandType = CommandType.StoredProcedure;
        //        objCommand.Transaction = objTransaction;
        //        objCommand.CommandText = SPName;
        //        foreach (var row in dt.Rows)
        //        {
        //            ht.Add(dt[row]
        //            if (parms.Count > 0)
        //            {
        //                foreach (DictionaryEntry de in parms)
        //                {
        //                    objCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value);
        //                }
        //            }
        //            objDataAdapter.SelectCommand = objCommand;
        //            objDataAdapter.Fill(dsReturn);

        //        }

        //        return dsReturn;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        this.InCloseConnection(true);
        //    }

        //}

        #endregion

        public string ExecuteProcudereWithParamsOutPut(string SPName, Hashtable parms)
        {
            string sResult = "";
            try
            {
                this.InOpenConnection(true);
                objCommand = new SqlCommand();
                objDataAdapter = new SqlDataAdapter();
                //System.Data.SqlClient.SqlParameter SqlPara = null;
                objCommand.Connection = objConnection;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Transaction = objTransaction;
                objCommand.CommandText = SPName;
                if (parms.Count > 0)
                {
                    foreach (DictionaryEntry de in parms)
                    {
                        if (de.Key.ToString() == "@iImgTransmittalHdrId" || de.Key.ToString() == "@iImgTransmittalDtlId" || de.Key.ToString() == "@iNodeId" || de.Key.ToString() == "@iModalityNo" || de.Key.ToString() == "@iAnatomyNo" || de.Key.ToString() == "@iNoImages" || de.Key.ToString() == "@iModifyBy" || de.Key.ToString() == "@iImageMode")
                        {
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.Int).Value = Convert.ToInt32(de.Value);
                        }
                        else if (de.Key.ToString() == "@dExaminationDate")
                        {
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.DateTime).Value = Convert.ToDateTime(de.Value);
                        }
                        else if (de.Key.ToString() == "@TransmittalImgDtl")
                        {
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.Structured).Value = de.Value;
                        }
                        else
                        {
                            objCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                        }
                    }
                    objCommand.Parameters.Add("@ReturnCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                    objCommand.ExecuteNonQuery();
                    int dsReturn = Convert.ToInt32(objCommand.Parameters["@ReturnCode"].Value);
                    //if (dsReturn == 1)
                    //{
                    //    sResult = "success - 1";
                    //}
                    sResult = Convert.ToString(dsReturn);
                }
                return sResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.InCloseConnection(true);
            }
        }

        public DataTable ExecuteProcudereWithParamsOutPut2(string SPName, Hashtable parms)
        {
            DataTable dt = new DataTable();
            try
            {
                this.InOpenConnection(true);
                objCommand = new SqlCommand();
                objDataAdapter = new SqlDataAdapter();
                //System.Data.SqlClient.SqlParameter SqlPara = null;
                objCommand.Connection = objConnection;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Transaction = objTransaction;
                objCommand.CommandText = SPName;
                if (parms.Count > 0)
                {
                    foreach (DictionaryEntry de in parms)
                    {
                        if (de.Key.ToString() == "@iNodeId" || de.Key.ToString() == "@DATAMODE" || de.Key.ToString() == "@iModifyBy")
                        {
                            //objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.VarChar).Value = Convert.ToString(de.Value);
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.Int).Value = Convert.ToInt32(de.Value);
                        }
                        else if (de.Key.ToString() == "@CRFTempDtl")
                        {
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.Structured).Value = de.Value;
                        }
                        else
                        {
                            objCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                        }
                    }
                    //objCommand.Parameters.Add("@ReturnCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                    objDataAdapter.SelectCommand = objCommand;
                    objDataAdapter.Fill(dt);

                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.InCloseConnection(true);
            }
        }

        //public DataTable GetDataTableWorkSpacenodedetailBiz(string TblName_1, string strWhere)
        //{
        //    DataTable Sql_DtTbl_Retu = new DataTable();
        //    var SqlAdp = new SqlDataAdapter();
        //    var Sql_Cmd = new SqlCommand();
        //    string qStr = "";

        //    TblName_1 = TblName_1.ToUpper();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(TblName_1))
        //        {
        //            throw new Exception("Invalid Table Ref.");
        //            return Sql_DtTbl_Retu;
        //        }
        //        qStr = "Select * From " + TblName_1 + " " + strWhere;


        //        InOpenConnectionBiz(true);
        //        Sql_Cmd = new SqlCommand(qStr, this.objConnectionBiz, this.objTransactionBiz);
        //        Sql_Cmd.CommandTimeout = 10000;
        //        SqlAdp = new SqlDataAdapter(Sql_Cmd);
        //        //Sql_DtTbl_Retu = new DataTable();
        //        SqlAdp.Fill(Sql_DtTbl_Retu);
        //        Sql_DtTbl_Retu.TableName = TblName_1;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        return Sql_DtTbl_Retu;
        //    }
        //    finally
        //    {
        //        InCloseConnectionBiz(true);
        //    }
        //    return Sql_DtTbl_Retu;

        //}

        public DataTable GetDataTableWorkSpacenodedetail(string TblName_1, string strWhere)
        {
            DataTable Sql_DtTbl_Retu = new DataTable();
            var SqlAdp = new SqlDataAdapter();
            var Sql_Cmd = new SqlCommand();
            string qStr = "";

            TblName_1 = TblName_1.ToUpper();
            try
            {
                if (string.IsNullOrEmpty(TblName_1))
                {
                    throw new Exception("Invalid Table Ref.");
                    return Sql_DtTbl_Retu;
                }
                qStr = "Select * From " + TblName_1 + " " + strWhere;


                InOpenConnection(true);
                Sql_Cmd = new SqlCommand(qStr, this.objConnection, this.objTransaction);
                Sql_Cmd.CommandTimeout = 10000;
                SqlAdp = new SqlDataAdapter(Sql_Cmd);
                //Sql_DtTbl_Retu = new DataTable();
                SqlAdp.Fill(Sql_DtTbl_Retu);
                Sql_DtTbl_Retu.TableName = TblName_1;

            }
            catch (Exception ex)
            {
                throw ex;
                return Sql_DtTbl_Retu;
            }
            finally
            {
                InCloseConnection(true);
            }
            return Sql_DtTbl_Retu;

        }

        public bool SaveInDbDataTable(string ProcedureName_1, DataTable tbl4Save, int Choice_1, ref ArrayList ProcedureOutPutVal_Retu, ref string eStr_Retu, bool SetTimeOut = false)
        {
            DataTable tbl_ProcDtl = null;
            System.Data.SqlClient.SqlParameter SqlPara = null;
            ParameterReturnValue ParaRetuVal = null;
            System.Data.SqlClient.SqlCommand sqlCmd = null;
            ArrayList ParaRowArr = null;
            string Proc_ColName = "";
            int ColIndex_1 = -1;

            try
            {
                ProcedureOutPutVal_Retu = new ArrayList();
                eStr_Retu = "";
                if (string.IsNullOrEmpty(ProcedureName_1.Trim()))
                {
                    eStr_Retu = "Procedure Name Is Invalid or Blank";
                    return false;
                }
                if (!getSqlObjectInfo(ProcedureName_1, ref tbl_ProcDtl, ref eStr_Retu))
                {
                    eStr_Retu = "Error Occured While Retrieving Procedure Info.";
                    return false;
                }
                if (tbl_ProcDtl.Rows.Count == 0)
                {
                    eStr_Retu = "No Procedure Information is found for procedure = " + ProcedureName_1;
                    return false;
                }

                foreach (DataRow dr_Save in tbl4Save.Rows)
                {
                    //this.InOpenConnectionBiz(true);
                    sqlCmd = new SqlCommand(ProcedureName_1, this.objConnection, this.objTransaction);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandTimeout = 180000;
                    ParaRowArr = new ArrayList();

                    foreach (DataRow dr_P in tbl_ProcDtl.Rows)
                    {
                        Proc_ColName = dr_P["COLNAME"].ToString().Substring(1);
                        ColIndex_1 = Convert.ToInt32(dr_P["COLINDEX"]) - 1;

                        SqlPara = new SqlParameter();
                        SqlPara.ParameterName = dr_P["COLNAME"].ToString();
                        SqlPara.Direction = ParameterDirection.Input;

                        if (Proc_ColName == "DATAOPMODE")
                        {
                            SqlPara.Value = Convert.ToInt32(Choice_1);
                        }
                        else if (Convert.ToInt32(dr_P["ISOUTPUT"]) == 0)
                        {
                            if (dr_Save.Table.Columns.Contains(Proc_ColName))
                            {
                                SqlPara.Value = dr_Save[Proc_ColName];
                            }
                        }
                        else if (Convert.ToInt32(dr_P["ISOUTPUT"]) == 1)
                        {
                            SqlPara.Direction = ParameterDirection.Output;
                            SqlPara.Size = Convert.ToInt32(dr_P["LENGTH"]);

                            ParaRetuVal = new ParameterReturnValue();
                            ParaRetuVal.ParameterName = Convert.ToString(dr_P["COLNAME"]);
                            ParaRetuVal.ParameterIndex = ColIndex_1;
                            ParaRowArr.Add(ParaRetuVal);
                        }

                        sqlCmd.Parameters.Add(SqlPara);
                    }

                    if (SetTimeOut)
                    {
                        sqlCmd.CommandTimeout = 180000;
                    }
                    sqlCmd.ExecuteNonQuery();

                    foreach (ParameterReturnValue retun_val in ParaRowArr)
                    {
                        ProcedureOutPutVal_Retu.Add((Convert.IsDBNull(sqlCmd.Parameters[retun_val.ParameterName].Value) ? "" : sqlCmd.Parameters[retun_val.ParameterName].Value));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ProcedureOutPutVal_Retu = null;
                eStr_Retu = ex.Message;
                return false;
            }
        }

        public bool SaveInDb(string ProcedureName_1, Hashtable tbl4Save, int Choice_1, ref ArrayList ProcedureOutPutVal_Retu, ref string eStr_Retu, bool SetTimeOut = false)
        {
            DataTable tbl_ProcDtl = null;
            System.Data.SqlClient.SqlParameter SqlPara = null;
            ParameterReturnValue ParaRetuVal = null;
            System.Data.SqlClient.SqlCommand sqlCmd = null;
            ArrayList ParaRowArr = null;
            string Proc_ColName = "";
            int ColIndex_1 = -1;

            try
            {
                ProcedureOutPutVal_Retu = new ArrayList();
                eStr_Retu = "";
                if (string.IsNullOrEmpty(ProcedureName_1.Trim()))
                {
                    eStr_Retu = "Procedure Name Is Invalid or Blank";
                    return false;
                }
                if (!getSqlObjectInfo(ProcedureName_1, ref tbl_ProcDtl, ref eStr_Retu))
                {
                    eStr_Retu = "Error Occured While Retrieving Procedure Info.";
                    return false;
                }
                if (tbl_ProcDtl.Rows.Count == 0)
                {
                    eStr_Retu = "No Procedure Information is found for procedure = " + ProcedureName_1;
                    return false;
                }

                this.InOpenConnection(true);
                sqlCmd = new SqlCommand(ProcedureName_1, this.objConnection, this.objTransaction);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                ParaRowArr = new ArrayList();

                foreach (DataRow dr_P in tbl_ProcDtl.Rows)
                {
                    Proc_ColName = dr_P["COLNAME"].ToString();
                    ColIndex_1 = Convert.ToInt32(dr_P["COLINDEX"]) - 1;

                    SqlPara = new SqlParameter();
                    SqlPara.ParameterName = dr_P["COLNAME"].ToString();
                    SqlPara.Direction = ParameterDirection.Input;

                    if (Proc_ColName == "DATAOPMODE")
                    {
                        SqlPara.Value = Convert.ToInt32(Choice_1);
                    }
                    else if (Convert.ToInt32(dr_P["ISOUTPUT"]) == 0)
                    {
                        SqlPara.Value = tbl4Save[Proc_ColName];
                    }
                    else if (Convert.ToInt32(dr_P["ISOUTPUT"]) == 1)
                    {
                        SqlPara.Direction = ParameterDirection.Output;
                        SqlPara.Size = Convert.ToInt32(dr_P["LENGTH"]);

                        ParaRetuVal = new ParameterReturnValue();
                        ParaRetuVal.ParameterName = Convert.ToString(dr_P["COLNAME"]);
                        ParaRetuVal.ParameterIndex = ColIndex_1;
                        ParaRowArr.Add(ParaRetuVal);
                    }

                    sqlCmd.Parameters.Add(SqlPara);
                }

                if (SetTimeOut)
                {
                    sqlCmd.CommandTimeout = 180000;
                }
                sqlCmd.ExecuteNonQuery();

                foreach (ParameterReturnValue retun_val in ParaRowArr)
                {
                    ProcedureOutPutVal_Retu.Add((Convert.IsDBNull(sqlCmd.Parameters[retun_val.ParameterName].Value) ? "" : sqlCmd.Parameters[retun_val.ParameterName].Value));
                }

                return true;
            }
            catch (Exception ex)
            {
                //return false;
                ProcedureOutPutVal_Retu = null;
                eStr_Retu = ex.Message;
                return false;
            }
            //return true;
        }

        private bool getSqlObjectInfo(string SqlObjectName_1, ref DataTable dtTbl_Retu, ref string eStr_Retu)
        {

            bool functionReturnValue = false;
            string qStr = "";

            try
            {
                SqlObjectName_1 = "'" + SqlObjectName_1.ToUpper() + "'";

                qStr = "Select SysColumns.Name as ColName, SysColumns.Colid as ColIndex, " +
                   " Type_Name(SysColumns.xType) as ColDbType, " +
                   " ColumnProperty(SysColumns.id, SysColumns.Name,'IsOutParam') as IsOutPut, " +
                   " ColumnProperty(SysColumns.id, SysColumns.Name,'IsIdentity') as IsAutoId, " +
                   " SysColumns.Length " +
                   " From SysColumns " +
                   " Where SysColumns.Id= Object_Id(" + SqlObjectName_1 + ")" + " " +
                   " Order by SysColumns.Colid";

                dtTbl_Retu = ExecuteQuery_DataSet(qStr).Tables[0];
                functionReturnValue = true;

            }
            catch (Exception ex)
            {
                functionReturnValue = false;
                eStr_Retu = ex.Message;
            }
            return functionReturnValue;

        }

        //private bool getSqlObjectInfoBiz(string SqlObjectName_1, ref DataTable dtTbl_Retu, ref string eStr_Retu)
        //{

        //    bool functionReturnValue = false;
        //    string qStr = "";

        //    try
        //    {
        //        SqlObjectName_1 = "'" + SqlObjectName_1.ToUpper() + "'";

        //        qStr = "Select SysColumns.Name as ColName, SysColumns.Colid as ColIndex, " +
        //           " Type_Name(SysColumns.xType) as ColDbType, " +
        //           " ColumnProperty(SysColumns.id, SysColumns.Name,'IsOutParam') as IsOutPut, " +
        //           " ColumnProperty(SysColumns.id, SysColumns.Name,'IsIdentity') as IsAutoId, " +
        //           " SysColumns.Length " +
        //           " From SysColumns " +
        //           " Where SysColumns.Id= Object_Id(" + SqlObjectName_1 + ")" + " " +
        //           " Order by SysColumns.Colid";

        //        dtTbl_Retu = ExecuteQuery_DataSetBiz(qStr).Tables[0];
        //        functionReturnValue = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        functionReturnValue = false;
        //        eStr_Retu = ex.Message;
        //    }
        //    return functionReturnValue;

        //}

        public enum DataObjOpenSaveModeEnum
        {
            //Enumeration Declaration for Open and Save Mode from Menu Page
            DataObjOpenMode_None = 0,
            DataObjOpenMode_Add = 1,
            DataObjOpenMode_Edit = 2,
            DataObjOpenMode_Delete = 3,
            DataObjOpenMode_View = 4,
            DataObjOpenMode_Rearrange = 5

        }

        public bool GetFieldsOfTable(string TableName, string Columns, string WhereCondition, ref DataSet Sql_DataSet, ref string eStr_Retu)
        {
            try
            {
                string qStr = "";
                if (string.IsNullOrEmpty(WhereCondition))
                {
                    qStr = "Select " + Columns + " from " + TableName;
                }
                else
                {
                    qStr = "Select " + Columns + " from " + TableName + " where " + WhereCondition;
                }
                Sql_DataSet = ExecuteQuery_DataSet(qStr);
                return true;
            }
            catch (Exception ex)
            {
                eStr_Retu = Convert.ToString(ex.Message);
                return false;
            }

        }

        public int ExecuteSP(string SPName, Hashtable parms)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                this.InOpenConnection(true);

                objCommand = new SqlCommand();
                objDataAdapter = new SqlDataAdapter();
                objCommand.Connection = objConnection;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Transaction = objTransaction;
                objCommand.CommandText = SPName;
                if (parms.Count > 0)
                {
                    foreach (DictionaryEntry de in parms)
                    {
                        objCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                    }
                }
                //objDataAdapter.SelectCommand = objCommand;
                //objDataAdapter.Fill(dsReturn);
                int result = objCommand.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //this.InCloseConnection(true);
            }
        }
        public string ExecuteProcedure(string SPName, Hashtable parms)
        {
            string sResult = "";
            try
            {
                //this.InOpenConnection(true);
                objCommand = new SqlCommand();
                objDataAdapter = new SqlDataAdapter();
                //System.Data.SqlClient.SqlParameter SqlPara = null;
                objCommand.Connection = objConnection;
                objCommand.CommandTimeout = 180000;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.Transaction = objTransaction;
                objCommand.CommandText = SPName;
                if (parms.Count > 0)
                {
                    foreach (DictionaryEntry de in parms)
                    {
                        if (de.Key.ToString() == "@iImgTransmittalHdrId" || de.Key.ToString() == "@iImgTransmittalDtlId" || de.Key.ToString() == "@iNodeId" || de.Key.ToString() == "@iModalityNo" || de.Key.ToString() == "@iAnatomyNo" || de.Key.ToString() == "@iNoImages" || de.Key.ToString() == "@iModifyBy" || de.Key.ToString() == "@iImageMode")
                        {
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.Int).Value = Convert.ToInt32(de.Value);
                        }
                        else if (de.Key.ToString() == "@dExaminationDate")
                        {
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.DateTime).Value = Convert.ToDateTime(de.Value);
                        }
                        else if (de.Key.ToString() == "@TransmittalImgDtl" || de.Key.ToString() == "@DicomAnnotationDtl")
                        {
                            objCommand.Parameters.Add(de.Key.ToString(), SqlDbType.Structured).Value = de.Value;
                        }
                        else
                        {
                            objCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                        }
                    }
                    objCommand.Parameters.Add("@ReturnCode", SqlDbType.Int).Direction = ParameterDirection.Output;
                    objCommand.Parameters.Add("@iTranNoReturn", SqlDbType.Int).Direction = ParameterDirection.Output;
                    objCommand.ExecuteNonQuery();
                    int dsReturn = Convert.ToInt32(objCommand.Parameters["@ReturnCode"].Value);
                    //if (dsReturn == 1)
                    //{
                    //    sResult = "success - 1";
                    //}
                    sResult = Convert.ToString(dsReturn);
                }
                return sResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //this.InCloseConnection(true);
            }
        }

    }
}
