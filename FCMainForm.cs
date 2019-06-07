/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using FaceCat;

namespace FaceCat {
    /// <summary>
    /// ����
    /// </summary>
    public partial class FCMainForm : Form {
        /// <summary>
        ///  ����ͼ�οؼ�
        /// </summary>
        public FCMainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// �ؼ�������
        /// </summary>
        private WinHost m_host;

        /// <summary>
        /// �ؼ���
        /// </summary>
        private FCNative m_native;

        /// <summary>
        /// ��ʱ��
        /// </summary>
        private int m_tick = 60;

        /// <summary>
        /// XML
        /// </summary>
        private UIXmlEx m_xml;

        /// <summary>
        /// ��ȡ�ͻ��˳ߴ�
        /// </summary>
        /// <returns>�ͻ��˳ߴ�</returns>
        public FCSize GetClientSize() {
            return new FCSize(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="name">����</param>
        public void loadXml(String name) {
            if (name == "MainFrame") {
                m_xml = new MainFrame();
            }
            m_xml.createNative();
            m_native = m_xml.Native;
            m_native.Paint = new GdiPlusPaintEx();
            m_host = new WinHostEx();
            m_host.Native = m_native;
            m_native.Host = m_host;
            m_host.HWnd = Handle;
            m_native.AllowScaleSize = true;
            m_native.DisplaySize = new FCSize(ClientSize.Width, ClientSize.Height);
            m_xml.resetScaleSize(GetClientSize());
            m_xml.Script = new FaceCatScript(m_xml);
            m_xml.Native.ResourcePath = DataCenter.getAppPath() + "\\config";
            m_xml.load(DataCenter.getAppPath() + "\\config\\" + name + ".html");
            m_host.ToolTip = new FCToolTip();
            m_host.ToolTip.Font = new FCFont("SimSun", 20, true, false, false);
            (m_host.ToolTip as FCToolTip).InitialDelay = 250;
            m_native.update();
            Invalidate();
        }

        /// <summary>
        /// ����ر��¼�
        /// </summary>
        /// <param name="e">�¼�����</param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            m_xml.exit();
            Environment.Exit(0);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            m_tick = 60;
        }

        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            m_tick = 60;
        }

        /// <summary>
        /// �ߴ�ı䷽��
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            if (m_host != null) {
                m_xml.resetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);
            if (m_host != null) {
                if (m_host.isKeyPress(0x11)) {
                    double scaleFactor = m_xml.ScaleFactor;
                    if (e.Delta > 0) {
                        if (scaleFactor > 0.2) {
                            scaleFactor -= 0.1;
                        }
                    }
                    else if (e.Delta < 0) {
                        if (scaleFactor < 10) {
                            scaleFactor += 0.1;
                        }
                    }
                    m_xml.ScaleFactor = scaleFactor;
                    m_xml.resetScaleSize(GetClientSize());
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void timer_Tick(object sender, EventArgs e) {
            m_tick--;
            if (m_tick <= 0) {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x100 || m.Msg == 260) {
                if (m_native != null) {
                    char key = (char)m.WParam;
                    if (m_xml is MainFrame) {
                        (m_xml as MainFrame).showSearchDiv(key);
                    }
                }
            }
            if (m_host != null) {
                if (m_host.onMessage(ref m) > 0) {
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
