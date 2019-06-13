using System;

public class Port_Scan_Data_Struct
{
	public Port_Scan_Data_Struct()
	{
	}

    //ICMP包结构
    public struct IcmpPacket
    {
        public byte Type;               //ICMP报文类型
        public byte SubCode;            //ICMP报文代码
        public ushort CheckSum;         //检验和
        public ushort Identifier;       //标识符
        public ushort SequenceNumber;   //序列码
        //public ushort Data;
    }
    //IP包结构
    public struct IP_HEADER
    {
        public byte VerLen;
        public byte ServiceType;
        public ushort TotalLen;
        public ushort ID;
        public ushort offset;
        public byte TimeToLive;
        public byte Protocol;
        public ushort HdrChksum;
        public uint SrcAddr;
        public uint DstAddr;
        //public byte Options;
    };
    //图形界面中显示表的数据结构
    public struct DataInfo
    {
        public string ip;
        public string port;
        public string stat;
        public DataInfo(string ip, string port, string stat)
        {
            this.ip = ip;
            this.port = port;
            this.stat = stat;
        }
    }
    //包含ip和端口的数据结构
    public struct IPPort
    {
        public string ip;
        public string port;
        public IPPort(string ip, string port)
        {
            this.ip = ip;
            this.port = port;
        }
    }
    //为了计算TCP，UDP checksum定义的伪首部
    public struct FAKE_HEADER       //定义TCP, UDP伪首部
    {
        public uint src;        //源地址
        public uint dst;        //目的地址
        public byte mbz;
        public byte ptcl;       //协议类型
        public ushort len;      //长度
    };
    //TCP包结构
    public struct TCP_HEADER
    {
        public ushort srcPort;
        public ushort dstPort;
        public uint seq;
        public uint ack;
        public byte headLen;
        public byte flag;
        public ushort windows;
        public ushort checkSum;
        public ushort urgency;
        public uint option;
        public uint option2;
    }
    //UDP包结构
    public struct UDP_HEADER
    {
        public ushort srcPort;
        public ushort dstPort;
        public ushort headLen;
        public ushort checkSum;
    }

}
