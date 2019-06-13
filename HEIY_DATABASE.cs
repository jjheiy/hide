//#define ACCESS
#define SQLSERVER

using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace heiy_db
{
    class Heiy
    {
#if SQLSERVER
        const string SQL_CONNECT_STR = "server=2405S-009; initial catalog=d1; user id=sa; password=123456";//连接字符串
        //const string SQL_CONNECT_STR = "server=DESKTOP-6T267TK; database=d1; integrated security=SSPI";//连接字符串
                static SqlConnection createConn(string sql_connect_str)
        {
            SqlConnection conn = new SqlConnection(sql_connect_str);
            return conn;
        }

#endif
#if ACCESS                                             //Jet.OLEDB.4.0      ACE.OLEDB.12.0
        const string SQL_CONNECT_STR = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=db.accdb";
        static OleDbConnection createConn(string sql_connect_str)
        {
            OleDbConnection conn = new OleDbConnection(sql_connect_str);
            return conn;
        }
#endif
        /// <summary>
        /// 此方法用于获取数据库的部分内容(看查询条件)
        /// </summary>
        /// <param name="ssql">sql查询语句</param>
        /// <param name="tableName">别名</param>
        /// <returns>DataTable</returns>

        public static DataTable selectTable(string ssql, string tableName)
        {   
#if SQLSERVER
            SqlConnection myconn = createConn(SQL_CONNECT_STR);
            SqlDataAdapter myda = new SqlDataAdapter(ssql, myconn);
#endif
#if ACCESS
            OleDbConnection myconn = createConn(SQL_CONNECT_STR);
            OleDbDataAdapter myda = new OleDbDataAdapter(ssql, myconn); 
#endif
            DataSet myds = new DataSet();

            myconn.Open();
            myda.Fill(myds, tableName);
            myconn.Close();
            return myds.Tables[tableName];
        }

        /// <summary>
        /// 此方法用于操作数据库，若执行成功，返回true;
        /// </summary>
        /// <param name="ssql">sql控制语句</param>
        /// <returns>bool</returns>
        public static bool changeTable(string ssql)
        {
#if SQLSERVER
            SqlConnection myconn = createConn(SQL_CONNECT_STR);
            try
            {
                SqlCommand mycmd = new SqlCommand(ssql, myconn);
#endif
#if ACCESS
            OleDbConnection myconn = createConn(SQL_CONNECT_STR);
            try
            {
                OleDbCommand mycmd = new OleDbCommand(ssql, myconn);
#endif       
                myconn.Open();
                mycmd.ExecuteNonQuery();
                myconn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 此方法用于判断字符串是否在字符串数组内，若在返回true
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="s_arr">字符数组</param>
        /// <returns>bool</returns>
	    public static bool isTrue(string s, string[] s_arr)//第一个参数是待判断字符串，第二个是字符串数组
	    {
		    foreach(string _s in s_arr){
		    	if(_s == s)               //若在字符串数组内找到与字符串相同的字符串，则返回true
			    	return true;
		    }
		    return false;
	    }

        /// <summary>
        /// 此方法用于将DataTable类型转换为字符串一位数组类型，方便使用
        /// </summary>
        /// <param name="dt">datatable</param>
        /// <returns>string[]</returns>
        public static string[] dtIsStr(DataTable dt)//参数为DataTable类型
        {
            string[] s_arr = new string[dt.Rows.Count*dt.Columns.Count];//字符串长度为DataTable类型的长宽之积
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    s_arr[i * dt.Columns.Count + j] = dt.Rows[i][j].ToString(); //迭代赋值即可
                }
            }
            return s_arr;
        }


        /// <summary>
        /// 此方法用于判断是否含有非法字符，有返回true
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ascllStart">起始ascll码</param>
        /// <param name="ascllEnd">终止ascll码</param>
        /// <returns>bool</returns>
        public static bool isHaveIllegalStr(string str, int ascllStart, int ascllEnd)
        {                                           //第一个参数是需要判断的字符串，第二个和第三个分别是字符有效的ascll范围
            char[] c_str = new char[str.Length];
            c_str = str.ToCharArray();
            foreach(char c in c_str)
            {
                if ((int)c < ascllStart && (int)c > ascllEnd) //如果字符串里含有范围外的字符，则返回true；
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// 重载，默认字母或数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>bool</returns>
        public static bool isHaveIllegalStr(string str)
        {                                           
            char[] c_str = new char[str.Length];
            c_str = str.ToCharArray();
            foreach (char c in c_str)
            {                           //  c的ascll范围为【48，57】U【65，90】U【97，122】
                if (((int)c > 122 || (int)c < 48) || ((int)c >57 && (int)c < 65) || ((int)c > 90 && (int)c < 97)) 
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// 创建文件,并填入内容
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="txt">写入内容</param>
        /// <returns>bool</returns>
        public static bool createFile(string path, string txt) 
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(txt);
                sw.Close();
                return true;
            }
            catch 
            {
                return false;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns>bool</returns>
        public static bool deleteFile(string path) 
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if (attr == FileAttributes.Directory)
                    Directory.Delete(path, true);
                else
                    File.Delete(path);
                return true;
            }
            catch {
                return false;
            }
        }
        /// <summary>
        /// 判断文件是否存在，若存在返回每一行的字符串组成的字符串数组，若不存在返回null
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>string[]</returns>
        public static string[] getFileTxt(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllLines(path);
            }
            else
            {
                return null;
            }
        }

        //public static void createLnk()
        //{
        //    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();

        //    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)+"\\随笔集.lnk");

        //    shortcut.TargetPath = "";
        //    shortcut.Arguments = Application.StartupPath + "\\test.exe";
        //    shortcut.Description = "哈喽，要打开吗";
        //    shortcut.Hotkey = "CTRL+SHIFT+S";
        //    shortcut.IconLocation = Application.StartupPath +"\\test.exe, 0";
        //    shortcut.Save();
        //}
    }
}