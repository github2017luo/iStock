/*����FaceCat(����è)��� v1.0 https://github.com/FaceCat007/facecat.git
 *  1.����è��ʼ��-�󶴳���Ա-�Ϻ����׿Ƽ���ʼ��-����KOL-�յ� (΢�ź�:suade1984);
 *  2.���ϴ�ʼ��-�Ϻ����׿Ƽ���ʼ��-Ԭ����(΢�ź�:wx627378127);
 *  3.��̩�ڻ�Ͷ����ѯ�ܼ�/�߼��о�Ա-������(΢�ź�:18345063201);
 *  4.���ϴ�ʼ��-Ф����(΢�ź�:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FaceCat {
    /// <summary>
    /// ��ѡ��
    /// </summary>
    public class UserSecurity {
        /// <summary>
        /// ����
        /// </summary>
        public String m_code;

        /// <summary>
        /// �Զ������
        /// </summary>
        public double m_buy;

        /// <summary>
        /// ״̬
        /// </summary>
        public int m_state;

        /// <summary>
        /// �Զ�������
        /// </summary>
        public double m_sell;

        /// <summary>
        /// ֹ���
        /// </summary>
        public double m_stop;
    }

    /// <summary>
    /// ��ѡ�ɷ���
    /// </summary>
    public class UserSecurityService {
        /// <summary>
        /// ��������
        /// </summary>
        public UserSecurityService() {
            UserCookie cookie = new UserCookie();
            UserCookieService cookieService = DataCenter.UserCookieService;
            if (cookieService.getCookie("USERSECURITY", ref cookie) > 0) {
                try {
                    m_codes = JsonConvert.DeserializeObject<List<UserSecurity>>(cookie.m_value);
                }
                catch (Exception ex) {
                }
                if (m_codes == null) {
                    try {
                        m_codes = JsonConvert.DeserializeObject<List<UserSecurity>>(cookie.m_value);
                    }
                    catch (Exception ex) {
                    }
                }
            }
        }

        /// <summary>
        /// ��ѡ����Ϣ
        /// </summary>
        public List<UserSecurity> m_codes = new List<UserSecurity>();

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="code">��ѡ��</param>
        public void add(UserSecurity code) {
            bool modify = false;
            int awardsSize = m_codes.Count;
            for (int i = 0; i < awardsSize; i++) {
                if (m_codes[i] == code) {
                    modify = true;
                    m_codes[i] = code;
                    save();
                    break;
                }
            }
            if (!modify) {
                m_codes.Add(code);
                save();
            }
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="code">����</param>
        public void delete(UserSecurity userSecurity) {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++) {
                if (m_codes[i].m_code == userSecurity.m_code) {
                    m_codes.RemoveAt(i);
                    save();
                    break;
                }
            }
        }

        /// <summary>
        /// ��ȡ�ν���Ϣ
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>�ν���Ϣ</returns>
        public UserSecurity get(String code) {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++) {
                if (m_codes[i].m_code == code) {
                    return m_codes[i];
                }
            }
            return null;
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public void save() {
            UserCookie cookie = new UserCookie();
            cookie.m_key = "USERSECURITY";
            cookie.m_value = JsonConvert.SerializeObject(m_codes);
            UserCookieService cookieService = DataCenter.UserCookieService;
            cookieService.addCookie(cookie);
        }
    }
}
