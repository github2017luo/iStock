/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Media;
using FaceCat;
using System.Threading;
using System.Runtime.InteropServices;

namespace FaceCat {
    /// <summary>
    /// ������
    /// </summary>
    public class Sound {
        /// <summary>
        /// ��ý����ƽӿڷ��Ϳ�������
        /// </summary>
        /// <param name="lpszCommand">����μ�
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd743572(v=vs.85).aspx </param>
        /// <param name="lpszReturnString">����ص���Ϣ�����û����Ҫ���ص���Ϣ����Ϊnull</param>
        /// <param name="cchReturn">ָ��������Ϣ���ַ�����С</param>
        /// <param name="hwndCallback">�ص������������������û��ָ��notify��ʶ������Ϊnew IntPtr(0)</param>
        /// <returns>��������ִ��״̬�Ĵ������</returns>
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String lpszCommand, StringBuilder returnString, int bufferSize, IntPtr hwndCallback);
        /// <summary>
        /// ���ض�ִ��״̬������������
        /// </summary>
        /// <param name="errorCode">mciSendCommand����mciSendString���صĴ������</param>
        /// <param name="errorText">�Դ������������ַ���</param>
        /// <param name="errorTextSize">ָ���ַ����Ĵ�С</param>
        /// <returns>���ERROR Codeδ֪������false</returns>
        [DllImport("winmm.dll")]
        static extern bool mciGetErrorString(Int32 errorCode, StringBuilder errorText, Int32 errorTextSize);

        private static Dictionary<String, String> m_plays = new Dictionary<String, String>();

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="args">����</param>
        private static void startPlay(object args) {
            String fileName = Application.StartupPath + "\\config\\" + args.ToString();
            if (FCFile.isFileExist(fileName)) {
                try {
                    int error = mciSendString("open " + fileName, null, 0, new IntPtr(0));
                    if (error == 0) {
                        mciSendString("play " + fileName, null, 0, new IntPtr(0));
                        Thread.Sleep(3000);
                        mciSendString("stop " + fileName, null, 0, new IntPtr(0));
                        mciSendString("close " + fileName, null, 0, new IntPtr(0));
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            lock (m_plays) {
                if (m_plays.ContainsKey(args.ToString())) {
                    m_plays.Remove(args.ToString());
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="key">�ļ���</param>
        public static void play(String key) {
            lock (m_plays) {
                if (!m_plays.ContainsKey(key)) {
                    m_plays[key] = "";
                    Thread thread = new Thread(new ParameterizedThreadStart(startPlay));
                    thread.Start(key);
                }
            }
        }
    }
}
