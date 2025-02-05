using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.ModbusLib;

namespace BMSproject
{
    /// <summary>
    /// 数据采集类，打开设备连接，关闭设备连接，采集设备数据，发送控制指令
    /// </summary>
    public class Datacommunication
    {
       private  ModbusRTU rtu = new ModbusRTU();

        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="PortName"></param>
        public void Connect(string PortName)//默认 9600,8,1,None,RTU
        {
            rtu.Connect(PortName);
        }

        /// <summary>
        /// 断开设备连接
        /// </summary> 
        public void Disconnect()
        {
            rtu.DisConnect();
        }

        public Dictionary<string, object> ReadVariableData(byte slaveId)//通过站地址读取变量数据
        {
            

            ushort[] ushorts= rtu.ReadDataArray(slaveId, 10,70);//读取寄存器数据
            if (ushorts == null) return null; //如果读取失败，返回null
            Dictionary<string, object> data = new Dictionary<string, object>()//将寄存器读取到的数据转换为字典存储起来
            {
                ["系统状态"] = ushorts[0],
                ["进网电压"] = ushorts[1],
                ["实时电流"] = ushorts[2]*0.1,
                ["总耗电量"] = ushorts[3] * 0.1,
                ["当前电价"] = ushorts[9] * 0.01,

                ["1号模组状态"] = ushorts[10],
                ["1号模组供电"] = ushorts[11],
                ["1号充电电压"] = ushorts[12],
                ["1号充电电流"] = ushorts[13]*0.1f,
                ["1号输出功率"] = ushorts[14],
                ["1号已用电量"] = ushorts[15] * 0.1f,
                ["1号充电时长"] = ushorts[16] * 0.01f,
                ["1号预计费用"] = ushorts[17] * 0.01f,
                ["1号电池电量"] = ushorts[18],

                ["2号模组状态"] = ushorts[20],
                ["2号模组供电"] = ushorts[21],
                ["2号充电电压"] = ushorts[22],
                ["2号充电电流"] = ushorts[23] * 0.1f,
                ["2号输出功率"] = ushorts[24],
                ["2号已用电量"] = ushorts[25] * 0.1f,
                ["2号充电时长"] = ushorts[26] * 0.01f,
                ["2号预计费用"] = ushorts[27] * 0.01f,
                ["2号电池电量"] = ushorts[28],

                ["3号模组状态"] = ushorts[30],
                ["3号模组供电"] = ushorts[31],
                ["3号充电电压"] = ushorts[32],
                ["3号充电电流"] = ushorts[33] * 0.1f,
                ["3号输出功率"] = ushorts[34],
                ["3号已用电量"] = ushorts[35] * 0.1f,
                ["3号充电时长"] = ushorts[36] * 0.01f,
                ["3号预计费用"] = ushorts[37] * 0.01f,
                ["3号电池电量"] = ushorts[38],

                ["4号模组状态"] = ushorts[40],
                ["4号模组供电"] = ushorts[41],
                ["4号充电电压"] = ushorts[42],
                ["4号充电电流"] = ushorts[43] * 0.1f,
                ["4号输出功率"] = ushorts[44],
                ["4号已用电量"] = ushorts[45] * 0.1f,
                ["4号充电时长"] = ushorts[46] * 0.01f,
                ["4号预计费用"] = ushorts[47] * 0.01f,
                ["4号电池电量"] = ushorts[48],

                ["5号模组状态"] = ushorts[50],
                ["5号模组供电"] = ushorts[51],
                ["5号充电电压"] = ushorts[52],
                ["5号充电电流"] = ushorts[53] * 0.1f,
                ["5号输出功率"] = ushorts[54],
                ["5号已用电量"] = ushorts[55] * 0.1f,
                ["5号充电时长"] = ushorts[56] * 0.01f,
                ["5号预计费用"] = ushorts[57]*0.01f,
                ["5号电池电量"] = ushorts[58],

                ["6号模组状态"] = ushorts[60],
                ["6号模组供电"] = ushorts[61],
                ["6号充电电压"] = ushorts[62],
                ["6号充电电流"] = ushorts[63] * 0.1f,
                ["6号输出功率"] = ushorts[64],
                ["6号已用电量"] = ushorts[65] * 0.1f,
                ["6号充电时长"] = ushorts[66]*0.01f,
                ["6号预计费用"] = ushorts[67] * 0.01f,
                ["6号电池电量"] = ushorts[68],
            };//对象初始化器
            return data;

        }




    }
}
