using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCP
{
	public TCP()
	{
	}
    //多线程端口扫描函数
    public void StartScan(object state)
    {
        Int32 port = (Int32)state;
        string tMsg = "";
        string getData = "";
        connState++;        //判断线程数目
        try
        {
            TcpClient tcp = new TcpClient();
            tcp.Connect(scanHost, port);
            //将扫描结果记录在数组中
            portSum++;
            tMsg = port.ToString() + "端口开放.";
            portListArray[portSum - 1] = tMsg;
            //获取服务协议类型
            Stream sm = tcp.GetStream();
            sm.Write(Encoding.Default.GetBytes(tMsg.ToCharArray()), 0, tMsg.Length);
            StreamReader sr = new StreamReader(tcp.GetStream(), Encoding.Default);
            try
            {
                getData = sr.ReadLine();
                //这行失败，无法读取协议信息，将直接跳转到catch语句
                if(getData.Length != 0)
                {
                    tMsg = port.ToString() + "端口数据：" + getData.ToString();
                    portListArray[portSum - 1] = tMsg;
                }
            }
            catch
            {

            }
            finally
            {
                sr.Close();
                sm.Close();
                tcp.Close();
                Thread.Sleep(0);        //指定表示应挂起该线程以及其他等待线程能够执行
            }
        }
        catch
        {

        }
        finally
        {
            Thread.Sleep(0);
            asyncOpsAreDone.Close();
        }
    }

}
