/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Threading;

namespace FaceCat {
    /// <summary>
    /// ָ����
    /// </summary>
    public class IndexDiv : FCView {
        /// <summary>
        /// �����ؼ�
        /// </summary>
        public IndexDiv() {
            BackColor = FCColor.argb(0, 0, 0);
            BorderColor = FCColor.None;
        }

        /// <summary>
        /// ��ҵ��ָ������
        /// </summary>
        private SecurityLatestData m_cyLatestData = new SecurityLatestData();

        /// <summary>
        /// ������
        /// </summary>
        private int m_requestID = FCClientService.getRequestID();

        /// <summary>
        /// ��ָ֤������
        /// </summary>
        private SecurityLatestData m_ssLatestData = new SecurityLatestData();

        /// <summary>
        /// ��ָ֤������
        /// </summary>
        private SecurityLatestData m_szLatestData = new SecurityLatestData();

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = FCView.getNewTimerID();

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public MainFrame MainFrame {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// �ؼ����ط���
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            startTimer(m_timerID, 1000);
        }

        /// <summary>
        /// ������¼�
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="button"></param>
        /// <param name="clicks"></param>
        /// <param name="delta"></param>
        public override void onClick(FCTouchInfo touchInfo) {
            FCPoint mp = touchInfo.m_firstPoint;
            base.onClick(touchInfo);
            int width = Width;
            String code = "";
            if (mp.x < width / 3) {
                code = m_ssLatestData.m_code;
            }
            else if (mp.x < width * 2 / 3) {
                code = m_szLatestData.m_code;
            }
            else {
                code = m_cyLatestData.m_code;
            }
            //m_mainFrame.searchSecurity(code);
        }

        /// <summary>
        /// ����ǰ������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            FCRect bounds = Bounds;
            int width = bounds.right - bounds.left;
            int height = bounds.bottom - bounds.top;
            if (width > 0 && height > 0) {
                if (m_ssLatestData != null && m_szLatestData != null && m_cyLatestData != null) {
                    long titleColor = FCColor.argb(255, 255, 80);
                    FCFont font = new FCFont("SimSun", 16, false, false, false);
                    FCFont indexFont = new FCFont("Arial", 14, true, false, false);
                    long grayColor = FCColor.Border;
                    //��ָ֤��
                    long indexColor = FCDraw.getPriceColor(m_ssLatestData.m_close, m_ssLatestData.m_lastClose);
                    int left = 1;
                    FCDraw.drawText(paint, "��֤", titleColor, font, left, 3);
                    left += 40;
                    paint.drawLine(grayColor, 1, 0, left, 0, left, height);
                    String amount = (m_ssLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    FCSize amountSize = paint.textSize(amount, indexFont);
                    FCDraw.drawText(paint, amount, titleColor, indexFont, width / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    int length = FCDraw.drawUnderLineNum(paint, m_ssLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = FCDraw.drawUnderLineNum(paint, m_ssLatestData.m_close - m_ssLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //��ָ֤��
                    left = width / 3;
                    paint.drawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = FCDraw.getPriceColor(m_szLatestData.m_close, m_szLatestData.m_lastClose);
                    FCDraw.drawText(paint, "��֤", titleColor, font, left, 3);
                    left += 40;
                    paint.drawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_szLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    amountSize = paint.textSize(amount, indexFont);
                    FCDraw.drawText(paint, amount, titleColor, indexFont, width * 2 / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    length = FCDraw.drawUnderLineNum(paint, m_szLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = FCDraw.drawUnderLineNum(paint, m_szLatestData.m_close - m_szLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //��ҵָ��
                    left = width * 2 / 3;
                    paint.drawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = FCDraw.getPriceColor(m_cyLatestData.m_close, m_cyLatestData.m_lastClose);
                    FCDraw.drawText(paint, "��ҵ", titleColor, font, left, 3);
                    left += 40;
                    paint.drawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_cyLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    amountSize = paint.textSize(amount, indexFont);
                    FCDraw.drawText(paint, amount, titleColor, indexFont, width - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    length = FCDraw.drawUnderLineNum(paint, m_cyLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4 + length;
                    length = FCDraw.drawUnderLineNum(paint, m_cyLatestData.m_close - m_cyLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    paint.drawRect(grayColor, 1, 0, new FCRect(0, 0, width - 1, height - 1));
                }
            }
        }

        /// <summary>
        /// �����
        /// </summary>
        /// <param name="timerID">���ID</param>
        public override void onTimer(int timerID) {
            if (m_timerID == timerID) {
                SecurityService.getLatestData("000001.SH", ref m_ssLatestData);
                SecurityService.getLatestData("399001.SZ", ref m_szLatestData);
                SecurityService.getLatestData("399006.SZ", ref m_cyLatestData);
                invalidate();
            }
        }
    }
}
