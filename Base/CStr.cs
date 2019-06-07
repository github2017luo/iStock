/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FaceCat {
    public class FCStrEx {
        /// <summary>
        /// ��ȡ֤ȯ������ļ�����
        /// </summary>
        /// <param name="code">����</param>
        /// <returns>�ļ�����</returns>
        public static String convertDBCodeToFileName(String code) {
            String fileName = code;
            if (fileName.IndexOf(".") != -1) {
                fileName = fileName.Substring(fileName.IndexOf('.') + 1) + fileName.Substring(0, fileName.IndexOf('.'));
            }
            fileName += ".txt";
            return fileName;
        }

        /// <summary>
        /// ����Ʊ����ת��Ϊ�ɽ�����
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <returns>���˴���</returns>
        public static String convertDBCodeToDealCode(String code) {
            String securityCode = code;
            int index = securityCode.IndexOf(".");
            if (index > 0) {
                securityCode = securityCode.Substring(0, index);
            }
            return securityCode;
        }

        /// <summary>
        /// ���ƴ���ת��Ϊ����
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static String convertEMCodeToDBCode(String code) {
            return code.Substring(code.IndexOf(".") + 1) + code.Substring(0, code.IndexOf("."));
        }

        /// <summary>
        /// ����Ʊ����ת��Ϊ���˴���
        /// </summary>
        /// <param name="code">��Ʊ����</param>
        /// <returns>���˴���</returns>
        public static String convertDBCodeToSinaCode(String code) {
            String securityCode = code;
            int index = securityCode.IndexOf(".SH");
            if (index > 0) {
                securityCode = "sh" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            else {
                securityCode = "sz" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            return securityCode;
        }

        /// <summary>
        /// ���ı��ļ��еĹ�Ʊ����ת�����ڴ��еĹ�Ʊ����
        /// </summary>
        /// <param name="code">�ļ��еĹ�Ʊ����</param>
        /// <returns>�ڴ��еĹ�Ʊ����</returns>
        public static String convertFileCodeToMemoryCode(String code) {
            int a = (code.IndexOf("."));
            return code.Substring(code.IndexOf(".") + 1, 2) + code.Substring(0, code.IndexOf(".")).ToLower();
        }

        /// <summary>
        /// �����˴���ת��Ϊ��Ʊ����
        /// </summary>
        /// <param name="code">���˴���</param>
        /// <returns>��Ʊ����</returns>
        public static String convertSinaCodeToDBCode(String code) {
            int equalIndex = code.IndexOf('=');
            int startIndex = code.IndexOf("var hq_str_") + 11;
            String securityCode = equalIndex > 0 ? code.Substring(startIndex, equalIndex - startIndex) : code;
            securityCode = securityCode.Substring(2) + "." + securityCode.Substring(0, 2).ToUpper();
            return securityCode;
        }

        /// <summary>
        /// ��ȡ���ݿ�ת���ַ���
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>ת���ַ���</returns>
        public static String getDBString(String str) {
            return str.Replace("'", "''");
        }
    }
}
