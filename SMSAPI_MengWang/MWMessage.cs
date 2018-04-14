using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.SMSAPI_MengWang
{
    public class MWMessage
    {
        /// <summary>
        /// 获取或设置用户账号
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        ///   获取或设置用户密码
        /// </summary>
        public String Pwd { get; set; }

        /// <summary>
        /// 
        ///  获取或设置目标号码，用英文逗号(,)分隔，最大1000个号码。一次提交的号码类型不受限制，但手机会做验证，若有不合法的手机号将会被退回。号码段类型分为：移动、联通、电信手机
        /// 注意：请不要使用中文的逗号
        /// </summary>
        public String Mobile { get; set; }

        /// <summary>
        ///   获取或设置短信内容， 内容长度不大于990个汉字
        /// </summary>
        public String Content { get; set; }

        /// <summary>
        ///  获取或设置时间戳
        /// 时间戳,格式为:MMDDHHMMSS,即月日时分秒,定长10位,月日时分秒不足2位时左补0.时间戳请获取您真实的服务器时间,不要填写固定的时间,否则pwd参数起不到加密作用
        /// 
        /// </summary>
        public String TimeStamp { get; set; }

        /// <summary>
        ///   获取或设置业务类型 最大可支持32位的字符串。预留字段，暂不生效。
        /// </summary>
        public String SvrType { get; set; }

        /// <summary>
        /// 
        ///  获取或设置子端口号码，不带请填星号{*}
        /// 长度由账号类型定4-6位，通道号总长度不能超过20位。如：10657****主通道号，3321绑定的扩展端口，主+扩展+子端口总长度不能超过20位。
        /// </summary>
        public String ExNo { get; set; }

        /// <summary>
        ///   获取或设置该条短信在您业务系统内的ID，比如订单号或者短信发送记录的流水号。填写后发送状态返回值内将包含这个ID.最大可支持64位的字符串
        /// </summary>
        public String CustId { get; set; }

        /// <summary>
        /// 
        ///  获取或设置请为您所发出每一个短信包进行惟一编号,网络异常需要重发时请保持不变，我们系统将根据包ID进行10分钟内的重复过滤.
        /// 若不需要滤重，可以不传packid或填0.
        /// 请尽量使用下面的方法保持packid的惟一性:
        /// 年年月月日日时时分分秒秒3位毫秒+1~99999的自增数字
        /// </summary>
        public long Packid { get; set; }

        /// <summary>
        ///  获取或设置额外提供的最大64个长度的自定义扩展数据.填写后发送状态返回值内将会包含这部分数据
        /// </summary> 
        public String ExData { get; set; }
        /// <summary>
        ///   获取或设置批量短信请求包。该字段中包含N个短信包结构体。每个结构体间用固定的分隔符隔开。
        /// </summary>
        public String Multimt { get; set; }
        /// <summary>
        ///  获取或设置
        /// N的范围为1~200.
        /// 分隔符为英文逗号(,).
        /// 单个短信包结构体的内容见下表.
        /// 获取上行或者状态报告的最大条数
        /// </summary>
        public int RetSize { get; set; }

        /// <summary>
        /// APIKEY
        /// </summary>
        public string ApiKey { get; set; }


        private IList<MultiMt> list;
        /// <summary>
        /// 不同内容群发
        /// </summary>
        public IList<MultiMt> Multimtes { get { return this.list; } set { this.list = value; } }
    }
}