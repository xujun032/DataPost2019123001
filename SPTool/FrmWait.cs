using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SPTool
{
     public partial  class FrmWait : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(386, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "正 在 处 理 请 稍 候 ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmWait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 60);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmWait";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public FrmWait()
        {
            InitializeComponent();
        }
    }

    public delegate void ExecutedInvokeHeandle();

    /// <summary>
    /// 等待窗口
    /// </summary>
    public class WaitForm
    {
        /// <summary>
        /// 等待窗口对象
        /// </summary>
        static FrmWait mWait;
        /// <summary>
        /// 事件状态通知
        /// </summary>
        static AutoResetEvent mEvents;
        /// <summary>
        /// 显示状态
        /// </summary>
        static bool mShowing = false;
        /// <summary>
        /// 等待状态
        /// </summary>
        static bool mWaiting = false;
        static WaitForm()
        {
            //全局等待信号
            mEvents = new AutoResetEvent(false);
        }
        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void WaitForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (mWaiting || !(sender is FrmWait))
            {
                //信号等待中被异常关闭,或主窗口被关闭时自动通知信号复位
                mEvents.Set();
                return;
            }
            mShowing = false;
            //回收等待窗口
            try
            {
                mWait.Dispose();
            }
            finally
            {
                mWait = null;
            }
        }
        /// <summary>
        /// 显示等待窗口
        /// </summary>
        /// <param name="pComponet">调用对象,可为控件或窗口</param>
        public static void Show(Control pComponet)
        {
            //已显示则返回
            if (mShowing)
                return;
            //创建等待窗口
            mWait = new FrmWait();
            //设置窗口关闭事件
            mWait.FormClosed += new FormClosedEventHandler(WaitForm_FormClosed);
            //启动显示线程
            Thread tmpThread = new Thread(show);
            tmpThread.Start(pComponet);
        }

  
        /// <summary>
        /// 关闭等待窗口
        /// </summary>
        public static void Close()
        {

            if (mShowing)
                //如果显示中则设置关闭信号
                mEvents.Set();
            //修改等待状态
            mWaiting = false;
        }
        /// <summary>
        /// 显示窗口过程
        /// </summary>
        /// <param name="pComponet"></param>
        static void show(object pComponet)
        {
            if (pComponet != null &&
                pComponet is Control)
            {
                //设置显示状态
                mShowing = true;
                //当调用对象为Win控件时
                (pComponet as Control).BeginInvoke(new ExecutedInvokeHeandle(delegate()
                {
                    //进行异步调用
                    if (pComponet != null &&
                        pComponet is Control)
                    {
                        //显示等待窗口
                        mWait.Show((pComponet as Control).FindForm());
                        //设置父级窗口关闭事件
                        (pComponet as Control).FindForm().FormClosed += new FormClosedEventHandler(WaitForm_FormClosed);
                        //激活父级窗口,
                        (pComponet as Control).FindForm().Activate();
                    }
                    else
                    {
                        //如查询不到父级窗口设置顶层显示,此处可用GetDesktop的API来获取桌面窗口,并设置为父级窗口
                        mWait.TopMost = true;
                        //显示等待窗口
                        mWait.Show();
                    }
                }));
                //设置等待信号
                mWaiting = true;
                mEvents.WaitOne();
                //等待结束
                mWaiting = false;
                (pComponet as Control).BeginInvoke(new ExecutedInvokeHeandle(delegate()
                {
                    //关闭窗口
                    mWait.Close();
                }));

            }
        }
    }
}
