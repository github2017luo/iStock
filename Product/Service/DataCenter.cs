/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// ������������
    /// </summary>
    public class DataCenter {
        private static ExportService m_exportService;

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public static ExportService ExportService {
            get { return m_exportService; }
        }

        private static bool m_isAppAlive = true;

        /// <summary>
        /// ��ȡ�����ó����Ƿ���
        /// </summary>
        public static bool IsAppAlive {
            get { return DataCenter.m_isAppAlive; }
            set { DataCenter.m_isAppAlive = value; }
        }

        /// <summary>
        /// ���߹���
        /// </summary>
        private static Dictionary<String, String> m_plots = new Dictionary<String, String>();

        /// <summary>
        /// ��ȡ���߹���
        /// </summary>
        public static Dictionary<String, String> Plots {
            get { return m_plots; }
        }

        private static UserCookieService m_userCookieService;

        /// <summary>
        /// �û�Cookie����
        /// </summary>
        public static UserCookieService UserCookieService {
            get { return DataCenter.m_userCookieService; }
        }

        private static UserSecurityService m_userSecurityService;

        /// <summary>
        /// ��ȡ��ѡ�ɷ���
        /// </summary>
        public static UserSecurityService UserSecurityService {
            get { return DataCenter.m_userSecurityService; }
        }

        static DataCenter() {
        }

        private static MainFrame m_mainUI;

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public static MainFrame MainUI {
            get { return DataCenter.m_mainUI; }
            set { DataCenter.m_mainUI = value; }
        }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns>����·��</returns>
        public static String getAppPath() {
            return Application.StartupPath;
        }

        /// <summary>
        /// ��ȡ�û�Ŀ¼
        /// </summary>
        /// <returns>�û�Ŀ¼</returns>
        public static String getUserPath() {
            return Application.StartupPath;
        }

        /// <summary>
        /// ��ȡ���еĻ��߹���
        /// </summary>
        private static void readPlots() {
            String xmlPath = Path.Combine(getAppPath(), "config\\Plots.xml");
            m_plots.Clear();
            if (File.Exists(xmlPath)) {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode rootNode = xmlDoc.DocumentElement;
                foreach (XmlNode node in rootNode.ChildNodes) {
                    if (node.Name.ToUpper() == "PLOT") {
                        String name = String.Empty;
                        String text = String.Empty;
                        foreach (XmlNode childeNode in node.ChildNodes) {
                            if (childeNode.Name.ToUpper() == "NAME") {
                                name = childeNode.InnerText;
                            }
                            else if (childeNode.Name.ToUpper() == "TEXT") {
                                text = childeNode.InnerText;
                            }
                        }
                        m_plots[name] = text;
                    }
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        public static void startService() {
            readPlots();
            m_userCookieService = new UserCookieService();
            m_exportService = new ExportService();
            m_userSecurityService = new UserSecurityService();
            SecurityService.start();
        }
    }
}
