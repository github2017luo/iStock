/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCat {
    /// <summary>
    /// ָ�갴ť
    /// </summary>
    public class IndicatorButton : FCButton {
        /// <summary>
        /// ������ť
        /// </summary>
        public IndicatorButton() {
            BorderColor = FCColor.None;
            Font = new FCFont("΢���ź�", 14, true, false, false);
        }

        /// <summary>
        /// �������
        /// </summary>
        private FCRect m_tRect = new FCRect();

        private bool m_isUser;

        /// <summary>
        /// ��ȡ�������Ƿ��û�ָ��
        /// </summary>
        public bool IsUser {
            get { return m_isUser; }
            set { m_isUser = value; }
        }

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ����������ؼ�
        /// </summary>
        public MainFrame MainFrame {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// �������·���
        /// </summary>
        /// <param name="touchInfo">������Ϣ</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            if (m_isUser) {
                FCPoint mp = touchInfo.m_firstPoint;
                if (mp.x >= m_tRect.left && mp.x <= m_tRect.right
                    && mp.y >= m_tRect.top && mp.y <= m_tRect.bottom) {
                    CreateWindow createWindow = new CreateWindow(Native);
                    createWindow.MainFrame = m_mainFrame;
                    createWindow.IndicatorData = Tag as IndicatorData;
                    createWindow.IsEdit = true;
                    createWindow.showDialog();
                }
            }
        }

        /// <summary>
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void onPaint(FCPaint paint, FCRect clipRect) {
            int vh = 0;
            FCLayoutDiv layoutDiv = Parent as FCLayoutDiv;
            if (layoutDiv.HScrollBar != null && layoutDiv.HScrollBar.Visible) {
                vh = layoutDiv.HScrollBar.Height;
            }
            int width = Width, height = Height - vh;
            FCRect drawRect = new FCRect(0, 0, width, height);
            paint.fillRect(FCColor.argb(0, 0, 0), drawRect);
            paint.drawRect(FCColor.argb(50, 105, 217), 1, 0, drawRect);
            String text = Text;
            FCFont font = Font;
            FCSize tSize = paint.textSize(text, font);
            FCRect tRect = new FCRect((width - tSize.cx) / 2, (height - tSize.cy) / 2, (width + tSize.cx) / 2, (height + tSize.cy) / 2);
            paint.drawText(text, FCColor.argb(255, 0, 0), font, tRect);
            FCPoint[] points = new FCPoint[3];
            points[0] = new FCPoint(0, 0);
            points[1] = new FCPoint(50, 0);
            points[2] = new FCPoint(0, 30);
            FCFont font3 = new FCFont("΢���ź�", 10, false, false, false);
            if (m_isUser) {
                paint.fillPolygon(FCColor.argb(255, 200, 0), points);
                FCDraw.drawText(paint, "�Զ���", FCColor.argb(0, 0, 0), font3, 2, 2);
                String btn1 = "�༭";
                FCFont font2 = new FCFont("΢���ź�", 12, false, false, false);
                FCSize tSize2 = paint.textSize(btn1, font2);
                tRect = new FCRect(width - tSize2.cx - 20, height - tSize2.cy - 5, 0, 0);
                tRect.right = tRect.left + tSize2.cx;
                tRect.bottom = tRect.top + tSize2.cy;
                m_tRect = tRect;
                paint.drawText(btn1, FCColor.argb(80, 255, 80), font2, m_tRect);
            } else {
                paint.fillPolygon(FCColor.argb(80, 255, 255), points);
                FCDraw.drawText(paint, "ϵͳ", FCColor.argb(0, 0, 0), font3, 2, 2);
            }
        }
    }
}
