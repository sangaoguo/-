using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BMSproject
{
    public partial class FrmMain : Form
    {
        private Datacommunication dc= new Datacommunication();         //实例化数据采集类对象
        private Dictionary<string, object> varValueDic = null;//新建一个成员变量，用于存储读取到的数据
        byte slaveId = 0;
        private ChartShow chartshow = null;//图表显示类
        List<ChartData> chartDataslist = new List<ChartData>();//存储图表数据
        int type = 1;


        void Reset()
        {
            lbl_totalVoltage.Text = "0";
            db_totalCurrent.CurrentValue = 0;
            lbl_totalKWH.Text = "0";

            foreach (GroupBox gb in this.Controls.OfType<GroupBox>())

            {
                if (gb.Tag != null && gb.Tag.ToString() == "tagdata")
                {
                    foreach (Label lbl in gb.Controls.OfType<Label>())
                        if (lbl.Tag != null && this.varValueDic[lbl.Tag.ToString()] != null)
                        {
                            lbl.Text = "0";
                        }
                }
            }//清空数据

            db_1Current.CurrentValue = 0;
            db_2Current.CurrentValue = 0;
            db_3Current.CurrentValue = 0;
            db_4Current.CurrentValue = 0;
            db_5Current.CurrentValue = 0;
            db_6Current.CurrentValue = 0;
            //清空电流表

            chartDataslist.Clear();//清空数据

            chartshow.ShowData(SeriesChartType.Candlestick, chartDataslist);



            foreach (Label lbl in this.gb7.Controls.OfType<Label>())
            {
                if (lbl.Tag != null && this.varValueDic[lbl.Tag.ToString()] != null)
                {
                    lbl.ForeColor=Color.Red;

                }
            }
        }
        public FrmMain()
        {
            InitializeComponent();
            this.cbb_PortName.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());//获取当前计算机的串口号
            chartshow = new ChartShow(this.chart1);//将要显示的图表控件传递给图表显示类
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            #region//判断用户输入的数据是否正确
            if (this.cbb_PortName.SelectedIndex == -1)      
            {
                MessageBox.Show("请选择正确的端口号","提示信息");
                return;//如果没有选择端口号，提示用户选择端口号
            }

            if (this.txtSlaveld.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入1到254区间的正确站地址号","提示信息");//如果没有输入站地址号，提示用户输入站地址号
                return;
            }
            
            try
            {
               slaveId=  Convert.ToByte(this.txtSlaveld.Text.Trim());//判断站地址是否是字节。如果不是字节，抛出异常
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入1到254区间的正确站地址号"+ex.Message, "提示信息");
                return;
            }
            #endregion
            #region//打开串口，采集数据,和关闭串口
            if (this.btnStart.Text == "开始监控")//打开串口，采集数据
            {
                try
                {
                     this.dc.Connect(this.cbb_PortName.Text);
                     btnStart.Text = "断开连接";
                    this.lblPortState.ForeColor = Color.Lime;
                    this.lblPortInfo.Text = "通信成功";
                    this.timerreaddata.Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("监控失败："+ex.Message);
                }
            }
            else//停止采集数据，关闭串口
            {
                this.timerreaddata.Stop();
                dc.Disconnect();
                this.lblPortState.ForeColor = Color.Red;
                this.lblPortInfo.Text = "未连接";
                btnStart.Text = "开始监控";
                Reset();
            }
            #endregion//
        }
        private void timerreaddata_Tick(object sender, EventArgs e)
        {
            
            varValueDic = dc.ReadVariableData(slaveId);//读取数据
            if (this.varValueDic == null || varValueDic.Count == 0) return;//判断

            #region//总数据显示
            this.lbl_totalVoltage.Text = this.varValueDic["进网电压"].ToString();
            this.db_totalCurrent.CurrentValue = (float)Math.Round(Convert.ToSingle(this.varValueDic["实时电流"].ToString()),2);
            this.lbl_totalKWH.Text = this.varValueDic["总耗电量"].ToString();
            #endregion

            #region//电流表显示
            this.db_1Current.CurrentValue = (float)Math.Round(Convert.ToSingle(this.varValueDic["1号充电电流"].ToString()),1);
            this.db_2Current.CurrentValue = (float)Math.Round(Convert.ToSingle(this.varValueDic["2号充电电流"].ToString()),1);
            this.db_3Current.CurrentValue = (float)Math.Round(Convert.ToSingle(this.varValueDic["3号充电电流"].ToString()), 1);
            this.db_4Current.CurrentValue = (float)Math.Round(Convert.ToSingle(this.varValueDic["4号充电电流"].ToString()), 1);
            this.db_5Current.CurrentValue = (float)Math.Round(Convert.ToSingle(this.varValueDic["5号充电电流"].ToString()), 1);
            this.db_6Current.CurrentValue = (float)Math.Round(Convert.ToSingle(this.varValueDic["6号充电电流"].ToString()), 1);
            #endregion

            #region//各个模组状态显示
            foreach ( Label lbl in this.gb7.Controls.OfType<Label>())
            {
                if (lbl.Tag != null && this.varValueDic[lbl.Tag.ToString()] != null)
                { 
                int state = Convert.ToInt16(this.varValueDic[lbl.Tag.ToString()]);
                    if (state == 0)
                    {
                        lbl.ForeColor= Color.Red;
                    }
                    else if(state==1)
                    {
                        lbl.ForeColor = Color.Lime;
                    }
                    else if (state == -1)
                    {
                        lbl.ForeColor = Color.DarkOrange;
                    }
                } 
            }
            foreach (GroupBox gb in this.Controls.OfType<GroupBox>())

            {
                if (gb.Tag != null &&gb.Tag.ToString()=="tagdata")
                {
                    foreach (Label lbl in gb.Controls.OfType<Label>())
                        if (lbl.Tag != null && this.varValueDic[lbl.Tag.ToString()] != null)
                {
                    lbl.Text = this.varValueDic[lbl.Tag.ToString()].ToString();
                }
                }
            }
            #endregion

            ShowChartData();//显示图表数据
        }

        private void ShowChartData()
        { 
        chartDataslist.Clear();//清空数据
            chartDataslist.Add(new ChartData("1号模组", Convert.ToDouble(this.varValueDic["1号电池电量"])));
            chartDataslist.Add(new ChartData("2号模组", Convert.ToDouble(this.varValueDic["2号电池电量"])));
            chartDataslist.Add(new ChartData("3号模组", Convert.ToDouble(this.varValueDic["3号电池电量"])));
            chartDataslist.Add(new ChartData("4号模组", Convert.ToDouble(this.varValueDic["4号电池电量"])));
            chartDataslist.Add(new ChartData("5号模组", Convert.ToDouble(this.varValueDic["5号电池电量"])));
            chartDataslist.Add(new ChartData("6号模组", Convert.ToDouble(this.varValueDic["6号电池电量"])));
            
            this.lblChangestyle1.BackColor=Color.Black;
            this.lblChangestyle2.BackColor = Color.Black;
            this.lblChangestyle3.BackColor = Color.Black;
            this.lblChangestyle4.BackColor = Color.Black;
            this.lblChangestyle5.BackColor = Color.Black;//清空颜色

            

            switch (type)
            {
                case 1:this.chartshow.ShowData(SeriesChartType.Column, chartDataslist); 
                    this.lblChangestyle1.BackColor = Color.Red;
                    break;
                case 2:
                    this.chartshow.ShowData(SeriesChartType.Radar, chartDataslist);
                    this.lblChangestyle2.BackColor = Color.Red;
                    break;
                case 3:
                    this.chartshow.ShowData(SeriesChartType.Doughnut, chartDataslist);
                    this.lblChangestyle3.BackColor = Color.Red;
                    break;
                case 4:
                    this.chartshow.ShowData(SeriesChartType.Line, chartDataslist);
                    this.lblChangestyle4.BackColor = Color.Red;
                    break;
                case 5:
                    this.chartshow.ShowData(SeriesChartType.Pie, chartDataslist);
                    this.lblChangestyle5.BackColor = Color.Red;
                    break;

            }
                
            
            

        }

        private void lblChangestyle1_Click(object sender, EventArgs e)
        {
            type = 1;
        }

        private void lblChangestyle2_Click(object sender, EventArgs e)
        {
            type = 2;
        }

        private void lblChangestyle3_Click(object sender, EventArgs e)
        {
            type = 3;
        }

        private void lblChangestyle4_Click(object sender, EventArgs e)
        {
            type = 4;
        }

        private void lblChangestyle5_Click(object sender, EventArgs e)
        {
            type = 5;
        }
    }
}
