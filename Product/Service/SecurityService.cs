/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Runtime.InteropServices;

namespace FaceCat {
    /// <summary>
    /// ��Ʊ����
    /// </summary>
    public class SecurityService {
        /// <summary>
        /// ������ֵ�
        /// </summary>
        public static Dictionary<String, Security> m_codedMap = new Dictionary<String, Security>();

        /// <summary>
        /// ��������
        /// </summary>
        public static Dictionary<String, List<SecurityData>> m_historyDatas = new Dictionary<String, List<SecurityData>>();

        /// <summary>
        /// ��������
        /// </summary>
        public static Dictionary<String, SecurityLatestData> m_latestDatas = new Dictionary<String, SecurityLatestData>();

        /// <summary>
        /// ���������ַ���
        /// </summary>
        private static Dictionary<String, String> m_latestDatasStr = new Dictionary<String, String>();

        /// <summary>
        /// ��ʱ����
        /// </summary>
        private static Dictionary<String, List<SecurityData>> m_minuteDatas = new Dictionary<String, List<SecurityData>>();

        /// <summary>
        /// ÿ�������ݴ��Ŀ¼
        /// </summary>
        private static String m_newFileDir = "";

        /// <summary>
        /// ��֤����ʱ��
        /// </summary>
        public static double m_shTradeTime;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        private static long m_today = 0;

        /// <summary>
        /// ����ʱ����������xls�����ļ�
        /// </summary>
        public static void downLoadSinaXlsDataByDate(String dateStr) {
            String contentPath = DataCenter.getAppPath() + "\\download\\xls\\";
            String urlTemplate = "http://market.finance.sina.com.cn/downxls.php?date={0}&symbol={1}";
            foreach (String code in m_codedMap.Keys) {
                String filePath = contentPath + code + "-" + dateStr + ".xls";
                String url = String.Format(urlTemplate, dateStr, code.ToLower());
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                Stream fs = new FileStream(filePath, FileMode.Create);
                while (size > 0) {
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                fs.Close();
                responseStream.Close();
            }
        }

        public static CList<Security> FilterCode(String text) {
            CList<Security> securities = new CList<Security>();
            foreach (Security gSecurity in SecurityService.m_codedMap.Values) {
                if (gSecurity.m_name != null) {
                    if (text.Length == 0) {
                        securities.push_back(gSecurity);
                    }
                    else {
                        if (gSecurity.m_code.ToUpper().IndexOf(text) == 0 || gSecurity.m_name.ToUpper().IndexOf(text) == 0
                            || gSecurity.m_pingyin.ToUpper().IndexOf(text) == 0) {
                            securities.push_back(gSecurity);
                        }
                    }
                }
            }
            return securities;
        }

        /// <summary>
        /// �������к�Լ
        /// </summary>
        /// <param name="codes">��Լ���뼯��</param>
        public static void getCodes(List<String> codes) {
            foreach (String key in m_codedMap.Keys) {
                codes.Add(key);
            }
        }

        /// <summary>
        /// ͨ�����뷵�غ�Լ��Ϣ
        /// </summary>
        /// <param name="code">��Լ����</param>
        /// <param name="sd">out ��Ʊ������Ϣ</param>
        public static int getSecurityByCode(String code, ref Security security) {
            int ret = 0;
            foreach (String key in m_codedMap.Keys) {
                if (key == code) {
                    security = m_codedMap[key];
                    ret = 1;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// ��ȡǰһ���µ��Ĺ�Ʊ
        /// </summary>
        public static List<String> getLastDayCodes(int type) {
            List<String> ret = new List<String>();
            foreach (String code in m_historyDatas.Keys) {
                List<SecurityData> sds = m_historyDatas[code];
                SecurityData sd = sds[sds.Count - 1];
                if (type == 0) {
                    if (sd.m_close < sd.m_open) {
                        ret.Add(code);
                    }
                }
                else if (type == 1) {
                    if (sd.m_close > sd.m_open) {
                        ret.Add(code);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="code">����</param>
        /// <param name="latestData">��������</param>
        /// <returns>״̬</returns>
        public static int getLatestData(String code, ref SecurityLatestData latestData) {
            int state = 0;
            lock (m_latestDatas) {
                if (m_latestDatas.ContainsKey(code)) {
                    latestData.copy(m_latestDatas[code]);
                    state = 1;
                }
            }
            return state;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="code">����</param>
        /// <param name="latestData">��������</param>
        /// <returns>״̬</returns>
        public static String getLatestData(String code) {
            lock (m_latestDatas) {
                if (m_latestDatasStr.ContainsKey(code)) {
                    return m_latestDatasStr[code];
                }
            }
            return "";
        }

        /// <summary>
        /// ������ʷ����
        /// </summary>
        /// <param name="history"></param>
        public static void loadHistoryDatas() {
            if (m_historyDatas.Count > 0) {
                return;
            }
            foreach (String code in m_codedMap.Keys) {
                String fileName = DataCenter.getAppPath() + "\\day\\" + FCStrEx.convertDBCodeToSinaCode(code).ToUpper() + ".txt";
                if (File.Exists(fileName)) {
                    StreamReader sra = new StreamReader(fileName, Encoding.Default);
                    String text = sra.ReadToEnd();
                    List<SecurityData> datas = new List<SecurityData>();
                    StockService.getHistoryDatasByTdxStr(text, datas);
                    if (datas.Count > 0) {
                        m_historyDatas[code] = datas;
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ�����Ծ�Ĵ���
        /// </summary>
        /// <returns>��Ծ���뼯��</returns>
        public static int getActiveCodes(List<String> activeCodes, int maxCount) {
            loadHistoryDatas();
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                //��ͣ��
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    if (m_historyDatas.ContainsKey(latestData.m_code)) {
                        List<SecurityData> datas = m_historyDatas[latestData.m_code];
                        int datasSize = datas.Count;
                        if (datasSize > 250) {
                            double totalVol = 0;
                            double latestVol = 0;
                            for (int i = 0; i < datasSize; i++) {
                                totalVol += datas[i].m_volume;
                                if (i > datasSize - 20) {
                                    latestVol += datas[i].m_volume;
                                }
                            }
                            if (totalVol > 0) {
                                dic[latestData.m_code] = latestVol / totalVol;
                            }
                        }
                    }
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < maxCount; i++) {
                activeCodes.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ��ʱ����
        /// </summary>
        public static void getMinuteDatas() {
            if (m_minuteDatas.Count > 0) {
                return;
            }
            String appPath = DataCenter.getAppPath();
            foreach (String code in m_codedMap.Keys) {
                String fileName = m_newFileDir + FCStrEx.convertDBCodeToFileName(code);
                if (!FCFile.isFileExist(fileName)) {
                    fileName = m_newFileDir + FCStrEx.convertDBCodeToSinaCode(code).ToUpper() + ".txt";
                }
                if (FCFile.isFileExist(fileName)) {
                    String text = "";
                    FCFile.read(fileName, ref text);
                    List<SecurityData> datas = new List<SecurityData>();
                    StockService.getHistoryDatasByMinuteStr(text, datas);
                    if (datas.Count > 0) {
                        int rindex = 0;
                        int dataSize = datas.Count;
                        while (rindex < dataSize) {
                            SecurityData d = datas[rindex];
                            if (rindex == 0) {
                                d.m_avgPrice = d.m_close;
                            }
                            else {
                                SecurityData ld = datas[rindex - 1];
                                d.m_avgPrice = (ld.m_avgPrice * rindex + d.m_close) / (rindex + 1);
                            }
                            rindex++;
                        }
                        m_minuteDatas[code] = datas;
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ��ͣ��Ʊ��Լ����
        /// </summary>
        /// <param name="limitUpCodes">out ��ͣ��Ʊ��Լ����</param>
        public static int getLimitUp(List<String> limitUpCodes) {
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                //��ͣ��
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    if (latestData.m_sellVolume1 == 0) {
                        if (latestData.m_buyVolume1 == 0) {
                            //�޽��׹�Ʊ
                            continue;
                        }
                        limitUpCodes.Add(latestData.m_code);
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ��ͣ��Լ����
        /// </summary>
        /// <param name="limitDownCodes">out ��ͣ��Լ����</param>
        public static int getLimitDown(List<String> limitDownCodes) {
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                //��ͣ��
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    if (latestData.m_buyVolume1 == 0) {
                        if (latestData.m_sellVolume1 == 0) {
                            //�޽��׹�Ʊ
                            continue;
                        }
                        limitDownCodes.Add(latestData.m_code);
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ�޽��׺�Լ����
        /// </summary>
        /// <param name="notTradedCodes">out  �޽��׺�Լ����</param>
        public static int getNotTradedCodes(List<String> notTradedCodes) {
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0 && latestData.m_buyVolume1 == 0 && latestData.m_sellVolume1 == 0) {
                    notTradedCodes.Add(latestData.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// ���׺�Լ����
        /// </summary>
        /// <param name="notTradedCodes">out  ���׺�Լ����</param>
        public static int getTradedCodes(List<String> tradedCodes) {
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    tradedCodes.Add(latestData.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// ���¹�
        /// </summary>
        /// <param name="secondNewCodes"></param>
        /// <returns></returns>
        public static int getSecondNewCodes(List<String> secondNewCodes, int maxCount) {
            loadHistoryDatas();
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                //��ͣ��
                if (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0) {
                    if (m_historyDatas.ContainsKey(latestData.m_code)) {
                        List<SecurityData> datas = m_historyDatas[latestData.m_code];
                        int datasSize = datas.Count;
                        if (datasSize > 20 && datasSize < 60) {
                            dic[latestData.m_code] = datasSize;
                        }
                    }
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s1.Value.CompareTo(s2.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < maxCount; i++) {
                secondNewCodes.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// ��ȡST��Ʊ
        /// </summary>
        /// <param name="stCodes">out ST��Ʊ</param>
        public static int getSTCodes(List<String> stCodes) {
            foreach (Security security in m_codedMap.Values) {
                if (security.m_name.IndexOf("ST") == 0) {
                    stCodes.Add(security.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ*ST��Ʊ
        /// </summary>
        /// <param name="stCodes">out ��ȡ*ST��Ʊ</param>
        public static int getStarSTCodes(List<String> starSTCodes) {
            foreach (Security security in m_codedMap.Values) {
                if (security.m_name.IndexOf("*ST") == 0) {
                    starSTCodes.Add(security.m_code);
                }
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ�Ƿ�����
        /// </summary>
        /// <param name="riseRanking">out �Ƿ�����</param>
        public static int getRiseRanking(List<String> riseRanking) {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    double rank = (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose;
                    dic.Add(latestData.m_code, rank);
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count; i++) {
                riseRanking.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="riseRanking">out ��������</param>
        public static int getFallRanking(ref List<String> fallRanking) {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    double rank = (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose;
                    dic.Add(latestData.m_code, rank);
                }
            }

            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s1.Value.CompareTo(s2.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count; i++) {
                fallRanking.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// ���ݳɽ�������
        /// </summary>
        /// <param name="codesByVol">�ɽ�������</param>
        public static int getCodesByVolume(List<String> codesByVol, int maxCount) {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    dic.Add(latestData.m_code, latestData.m_volume);
                }
            }

            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < maxCount; i++) {
                codesByVol.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// ���ݳɽ�������
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static int getCodesByAmount(List<String> codesByAmount, int maxCount) {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    dic.Add(latestData.m_code, latestData.m_amount);
                }
            }

            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < maxCount; i++) {
                codesByAmount.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static int getCodesBySwing(List<String> codesBySwing, int maxCount) {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0 && (latestData.m_buyVolume1 != 0 || latestData.m_sellVolume1 != 0)) {
                    if (latestData.m_lastClose > 0) {
                        dic.Add(latestData.m_code, (int)(latestData.m_high - latestData.m_low) / latestData.m_lastClose);
                    }
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s2.Value.CompareTo(s1.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < maxCount; i++) {
                codesBySwing.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// ���ݹɼ�����
        /// </summary>
        /// <param name="codesByAmount"></param>
        public static int getCodesByPrice(List<String> codesByPrice, int maxCount) {
            Dictionary<String, double> dic = new Dictionary<String, double>();
            foreach (SecurityLatestData latestData in m_latestDatas.Values) {
                if (latestData.m_close > 0) {
                    dic.Add(latestData.m_code, latestData.m_close);
                }
            }
            List<KeyValuePair<String, double>> lst = new List<KeyValuePair<String, double>>(dic);
            lst.Sort(delegate(KeyValuePair<String, double> s1, KeyValuePair<String, double> s2)
            {
                return s1.Value.CompareTo(s2.Value);
            });
            dic.Clear();
            for (int i = 0; i < lst.Count && i < maxCount; i++) {
                codesByPrice.Add(lst[i].Key);
            }
            return 1;
        }

        /// <summary>
        /// �����б�
        /// </summary>
        private static String m_codes;

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public static void start() {
            //���ش����//step 1
            m_codes = "";
            if (m_codedMap.Count == 0) {
                String content = "";
                FCFile.read(DataCenter.getAppPath() + "\\codes.txt", ref content);
                String[] strs = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String str in strs) {
                    String[] substrs = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    Security security = new Security();
                    security.m_code = substrs[0];
                    security.m_name = substrs[1];
                    m_codedMap[security.m_code] = security;
                    m_codes += security.m_code;
                    m_codes += ",";
                }
                loadHistoryDatas();
            }
            Thread thread = new Thread(new ThreadStart(startWork));
            thread.Start();
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private static void startWork() {
            while (true) {
                if (m_codes != null && m_codes.Length > 0) {
                    if (m_codes.EndsWith(",")) {
                        m_codes.Remove(m_codes.Length - 1);
                    }
                    String[] strCodes = m_codes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    int codesSize = strCodes.Length;
                    String latestCodes = "";
                    for (int i = 0; i < codesSize; i++) {
                        latestCodes += strCodes[i];
                        if (i == codesSize - 1 || (i > 0 && i % 50 == 0)) {
                            String latestDatasResult = StockService.getSinaLatestDatasStrByCodes(latestCodes);
                            if (latestDatasResult != null && latestDatasResult.Length > 0) {
                                List<SecurityLatestData> latestDatas = new List<SecurityLatestData>();
                                StockService.getLatestDatasBySinaStr(latestDatasResult, 0, latestDatas);
                                String[] subStrs = latestDatasResult.Split(new String[] { ";\n" }, StringSplitOptions.RemoveEmptyEntries);
                                int latestDatasSize = latestDatas.Count;
                                for (int j = 0; j < latestDatasSize; j++) {
                                    SecurityLatestData latestData = latestDatas[j];
                                    if (latestData.m_close == 0) {
                                        latestData.m_close = latestData.m_buyPrice1;
                                    }
                                    if (latestData.m_close == 0) {
                                        latestData.m_close = latestData.m_sellPrice1;
                                    }
                                    lock (m_latestDatas) {
                                        m_latestDatasStr[latestData.m_code] = subStrs[j];
                                        //bool append = true;
                                        //if (m_latestDatas.ContainsKey(latestData.m_code))
                                        //{
                                        //    if (!m_latestDatas[CStrA.ConvertFileCodeToMemoryCode(latestData.m_code)].Equals(latestData))
                                        //    {
                                        //        append = false;
                                        //    }
                                        //}
                                        //if(append)
                                        //{
                                        //    long today = (long)DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds / 86400000;
                                        //    if (m_today < today)
                                        //    {
                                        //        m_today = today;
                                        //        String nPath = DataCenter.GetAppPath() + "\\tick\\" + DateTime.Now.ToString("yyyy-MM-dd");
                                        //        if (!Directory.Exists(nPath))
                                        //        {
                                        //            Directory.CreateDirectory(nPath);
                                        //        }
                                        //        m_newFileDir = nPath + "\\";
                                        //    }
                                        //    String line = String.Format("{0},{1},{2},{3}\r\n", latestData.m_date,//
                                        //        latestData.m_close, latestData.m_volume, latestData.m_amount);
                                        //    CFileA.Append(m_newFileDir + latestData.m_code + ".txt", line);
                                        //}
                                        if (!m_latestDatas.ContainsKey(latestData.m_code)) {
                                            m_latestDatas[latestData.m_code] = latestData;
                                        }
                                        else {
                                            m_latestDatas[latestData.m_code].copy(latestData);
                                        }
                                        if (latestData.m_code == "000001.SH") {
                                            m_shTradeTime = latestData.m_date;
                                        }
                                    }
                                }
                                latestDatas.Clear();
                            }
                            latestCodes = "";
                        }
                        else {
                            latestCodes += ",";
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// ��������̹߳���
        /// </summary>
        public static void startWork3() {
            //��������
            loadHistoryDatas();
            //getMinuteDatas();
            //�¾����ݺϲ�
            foreach (String oCode in m_historyDatas.Keys) {
                if (!m_latestDatas.ContainsKey(oCode) || !m_historyDatas.ContainsKey(oCode)) continue;
                SecurityLatestData securityLatestData = m_latestDatas[oCode];
                List<SecurityData> oldSecurityDatas = m_historyDatas[oCode];
                SecurityData oldSecurityData = oldSecurityDatas[oldSecurityDatas.Count - 1];
                int myear = 0, mmonth = 0, mday = 0, mhour = 0, mmin = 0, msec = 0, mmsec = 0;
                FCStr.getDateByNum(oldSecurityData.m_date, ref myear, ref mmonth, ref mday, ref mhour, ref mmin, ref msec, ref mmsec);
                int year = 0, month = 0, day = 0, hour = 0, min = 0, sec = 0, msec2 = 0;
                FCStr.getDateByNum(securityLatestData.m_date, ref year, ref month, ref day, ref hour, ref min, ref sec, ref msec2);
                if (year >= myear && month >= mmonth && day >= mday) {
                    SecurityData nSecurityData = new SecurityData();
                    nSecurityData.m_amount = securityLatestData.m_amount;
                    nSecurityData.m_close = securityLatestData.m_close;
                    nSecurityData.m_date = securityLatestData.m_date;
                    nSecurityData.m_high = securityLatestData.m_high;
                    nSecurityData.m_low = securityLatestData.m_low;
                    nSecurityData.m_open = securityLatestData.m_open;
                    nSecurityData.m_volume = securityLatestData.m_volume;
                    if (day == mday) {
                        m_historyDatas[oCode].RemoveAt(m_historyDatas[oCode].Count - 1);
                    }
                    m_historyDatas[oCode].Add(nSecurityData);
                }
            }
            String outputFileTemplate = DataCenter.getAppPath() + "\\day\\{0}.txt";
            String fileInfo = "{0} {1} ���� ǰ��Ȩ\r\n";
            String title = "      ����	    ����	    ���	    ���	    ����	    �ɽ���	    �ɽ���\r\n";
            String lineTemp = "{0},{1},{2},{3},{4},{5},{6}\r\n";
            String timeFormatStr = "yyyy-MM-dd";
            //д���ļ�
            foreach (String code in m_historyDatas.Keys) {
                List<SecurityData> temp3 = m_historyDatas[code];
                StringBuilder strbuff = new StringBuilder();
                strbuff.Append(String.Format(fileInfo, m_codedMap[code].m_code, m_codedMap[code].m_name));
                strbuff.Append(title);
                foreach (SecurityData sdt in temp3) {
                    strbuff.Append(String.Format(lineTemp,//
                        FCStr.convertNumToDate(sdt.m_date).ToString(timeFormatStr),//
                        sdt.m_open,//
                        sdt.m_high,//
                        sdt.m_low,//
                        sdt.m_close,//
                        sdt.m_volume,//
                        sdt.m_amount));
                }
                strbuff.Append("������Դ:ͨ����\r\n");
                FCFile.write(String.Format(outputFileTemplate, code), strbuff.ToString());
            }
        }
    }
}
