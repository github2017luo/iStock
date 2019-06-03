/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using FaceCat;
using Newtonsoft.Json;

namespace FaceCat {
    /// <summary>
    /// ��������
    /// </summary>
    public class CreateWindow : WindowXmlEx {
        /// <summary>
        /// ������¼����
        /// </summary>
        /// <param name="native">������</param>
        public CreateWindow(FCNative native) {
            load(native, "CreateWindow", "windowCreate");
            //ע�����¼�
            registerEvents(m_window);
        }

        private IndicatorData m_indicatorData;

        /// <summary>
        /// ��ȡ������ָ������
        /// </summary>
        public IndicatorData IndicatorData {
            get { return m_indicatorData; }
            set { m_indicatorData = value; }
        }

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ����������ؼ�
        /// </summary>
        public MainFrame MainFrame {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private bool m_isEdit;

        /// <summary>
        /// ��ȡ�������Ƿ��Ǳ༭״̬
        /// </summary>
        public bool IsEdit {
            get { return m_isEdit; }
            set { m_isEdit = value; }
        }

        /// <summary>
        /// ��ť����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        private void clickButton(object sender, FCTouchInfo touchInfo) {
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                FCView control = sender as FCView;
                String name = control.Name;
                if (name == "btnCancel") {
                    close();
                } else if (name == "btnOK") {
                    FCTextBox txtName = getTextBox("txtName");
                    FCTextBox txtScript = getTextBox("txtScript");
                    List<IndicatorData> indicatorDatas = m_mainFrame.m_indicators;
                    IndicatorData newIndicatorData = null;
                    //�ж��Ƿ��ظ�
                    int indicatorDatasSize = indicatorDatas.Count;
                    for (int i = 0; i < indicatorDatasSize; i++) {
                        IndicatorData indicatorData = indicatorDatas[i];
                        if (indicatorData.m_name == txtName.Text) {
                            if (m_isEdit) {
                                newIndicatorData = indicatorData;
                            } else {
                                MessageBox.Show("��ָ�������Ѿ�����!", "��ʾ");
                                return;
                            }
                        }
                    }
                    //���ָ��
                    if (!m_isEdit) {
                        newIndicatorData = new IndicatorData();
                        indicatorDatas.Add(newIndicatorData);
                    }
                    newIndicatorData.m_name = txtName.Text;
                    newIndicatorData.m_script = txtScript.Text;
                    String path = Application.StartupPath + "\\indicators.txt";
                    String content = JsonConvert.SerializeObject(indicatorDatas);
                    FCFile.write(path, content);
                    if (!m_isEdit) {
                        m_mainFrame.addIndicator(newIndicatorData);
                    }
                    close();
                    Native.update();
                    Native.invalidate();
                } else if (name == "lblDelete") {
                    m_mainFrame.removeIndicator(m_indicatorData);
                    close();
                }
            }
        }

        /// <summary>
        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void registerEvents(FCView control) {
            FCTouchEvent clickButtonEvent = new FCTouchEvent(clickButton);
            List<FCView> controls = control.getControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++) {
                FCButton button = controls[i] as FCButton;
                FCLabel linkLabel = controls[i] as FCLabel;
                if (button != null) {
                    button.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                else if (linkLabel != null) {
                    linkLabel.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                registerEvents(controls[i]);
            }
        }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        public override void showDialog() {
            base.showDialog();
            FCButton btnOK = getButton("btnOK");
            FCButton btnCancel = getButton("btnCancel");
            FCLabel lblDelete = getLabel("lblDelete");
            if (m_isEdit) {
                FCTextBox txtName = getTextBox("txtName");
                FCTextBox txtScript = getTextBox("txtScript");
                txtName.Text = m_indicatorData.m_name;
                txtScript.Text = m_indicatorData.m_script;
                txtName.Enabled = false;
                lblDelete.Visible = true;
                btnOK.Text = "�޸�";
                m_window.Text = "�޸�ָ��";
            } else {
                lblDelete.Visible = false;
                btnOK.Text = "����";
                m_window.Height -= 50;
            }
            Native.invalidate();
        }
    }
}
