/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// ѡ�ɵ��ⲿ����
    /// </summary>
    public class SecurityFilterExternFunc {
        /// <summary>
        /// ��Ʊ�ؼ�
        /// </summary>
        private static FCChart m_chart;

        /// <summary>
        /// ������
        /// </summary>
        private static FCNative m_native;

        /// <summary>
        /// ��ˮ��
        /// </summary>
        private static int m_serialNumber;

        /// <summary>
        /// ָ�꼯��
        /// </summary>
        private static Dictionary<int, FCScript> m_indicators = new Dictionary<int, FCScript>();

        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="text">�ű�</param>
        /// <param name="parameters">����</param>
        /// <returns>ָ��ID</returns>
        public static int createIndicatorExtern(String text, String parameters, StringBuilder fields) {
            try {
                if (m_native == null) {
                    m_native = new FCNative();
                }
                if (m_chart == null) {
                    m_chart = new FCChart();
                    m_chart.Native = m_native;
                }
                m_serialNumber++;
                FCDataTable dataSource = new FCDataTable();
                dataSource.addColumn(KeyFields.CLOSE_INDEX);
                dataSource.addColumn(KeyFields.HIGH_INDEX);
                dataSource.addColumn(KeyFields.LOW_INDEX);
                dataSource.addColumn(KeyFields.OPEN_INDEX);
                dataSource.addColumn(KeyFields.VOL_INDEX);
                dataSource.addColumn(KeyFields.AMOUNT_INDEX);
                FCScript indicator = SecurityDataHelper.createIndicator(m_chart, dataSource, text, parameters);
                m_indicators[m_serialNumber] = indicator;
                indicator.onCalculate(0);
                int pos = 0;
                int variablesSize = indicator.MainVariables.Count;
                foreach (String field in indicator.MainVariables.Keys) {
                    fields.Append(field);
                    if (pos != variablesSize - 1) {
                        fields.Append(",");
                    }
                    pos++;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            return m_serialNumber;
        }

        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="id">ָ��ID</param>
        /// <param name="code">����</param>
        /// <param name="path">·��</param>
        /// <param name="type">����</param>
        /// <param name="cycle">����</param>
        /// <param name="subscription">��Ȩ��ʽ</param>
        /// <param name="date">����</param>
        /// <param name="open">���̼�</param>
        /// <param name="high">��߼�</param>
        /// <param name="low">��ͼ�</param>
        /// <param name="close">���̼�</param>
        /// <param name="volume">�ɽ���</param>
        /// <param name="amount">�ɽ���</param>
        /// <returns>��������</returns>
        public static double[] calculateIndicatorExtern(int id, String code, ref double result) {
            if (m_indicators.ContainsKey(id)) {
                FCScript indicator = m_indicators[id];
                List<FCScript> indicators = new List<FCScript>();
                indicators.Add(indicator);
                List<SecurityData> datas = new List<SecurityData>();
                if (SecurityService.m_historyDatas.ContainsKey(code)) {
                    datas = SecurityService.m_historyDatas[code];
                    SecurityLatestData latestData = null;
                    if (SecurityService.m_latestDatas.ContainsKey(code)) {
                        latestData = SecurityService.m_latestDatas[code];
                    }
                    if (latestData != null) {
                        SecurityData newData = new SecurityData();
                        getSecurityData(latestData, latestData.m_lastClose, 1440, 0, ref newData);
                        if (datas.Count == 0) {
                            datas.Add(newData);
                        } else {
                            if (newData.m_date > datas[datas.Count - 1].m_date) {
                                datas.Add(newData);
                            } else {
                                datas[datas.Count - 1] = newData;
                            }
                        }
                    }
                    FCDataTable dataSource = indicator.DataSource;
                    int[] fields = new int[] { KeyFields.CLOSE_INDEX, KeyFields.HIGH_INDEX, KeyFields.LOW_INDEX, KeyFields.OPEN_INDEX, KeyFields.VOL_INDEX, KeyFields.AMOUNT_INDEX };
                    SecurityDataHelper.bindHistoryDatas(m_chart, dataSource, indicators, fields, datas);;
                    int rowsCount = dataSource.RowsCount;
                    int variablesSize = indicator.MainVariables.Count;
                    double[] list = new double[variablesSize];
                    if (rowsCount > 0) {
                        int pos = 0;
                        foreach (String name in indicator.MainVariables.Keys) {
                            int field = indicator.MainVariables[name];
                            double value = dataSource.get2(rowsCount - 1, field);
                            list[pos] = value;
                            pos++;
                        }
                    }
                    result = indicator.m_result;
                    dataSource.clear();
                    return list;
                }
            } 
            return null;
        }

        /// <summary>
        /// ɾ��ָ��
        /// </summary>
        /// <param name="id">ָ��ID</param>
        public static void deleteIndicatorExtern(int id) {
            if (m_indicators.ContainsKey(id)) {
                FCScript indicator = m_indicators[id];
                m_indicators.Remove(id);
                indicator.clear();
                indicator.DataSource.delete();
                indicator.DataSource = null;
                indicator.delete();
            }
        }

        /// <summary>
        /// ��ȡ�зֺ������
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="divide">�з�ֵ</param>
        /// <returns></returns>
        public static double getDivideDate(double date, long divide) {
            return (double)((long)date / divide);
        }

        /// <summary>
        /// ��ȡ��Ʊ��ʷ����
        /// </summary>
        /// <param name="latestData">��������</param>
        /// <param name="lastClose">��һ�����̼�</param>
        /// <param name="cycle">����</param>
        /// <param name="subscription">��Ȩģʽ</param>
        /// <param name="securityData">��ʷ����</param>
        /// <returns>��ʷ����</returns>
        public static void getSecurityData(SecurityLatestData latestData, double lastClose, int cycle, int subscription, ref SecurityData securityData) {
            if (cycle <= 60) {
                securityData.m_date = getDivideDate(latestData.m_date, 60 * 60);
            } else {
                securityData.m_date = (long)latestData.m_date / (3600 * 24) * (3600 * 24);
            }
            //ǰ��Ȩ����
            double factor = 1;
            if (lastClose > 0 && latestData.m_lastClose > 0 && subscription == 2) {
                factor = lastClose / latestData.m_lastClose;
            }
            securityData.m_close = latestData.m_close * factor;
            securityData.m_high = latestData.m_high * factor;
            securityData.m_low = latestData.m_low * factor;
            securityData.m_open = latestData.m_open * factor;
            securityData.m_volume = latestData.m_volume;
            securityData.m_amount = latestData.m_amount;
        }
    }
}
