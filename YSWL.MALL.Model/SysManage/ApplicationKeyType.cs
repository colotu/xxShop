/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。 
//
// 文件名：ApplicationKeyType.cs
// 文件功能描述：
// 
// 创建标识：
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/


namespace YSWL.MALL.Model.SysManage
{
    public enum ApplicationKeyType
    {
        None = -1,
        System = 1,
        SNS = 2,
        Shop = 3,
        CMS = 4, //DONE: BEN MODIFY 根据 SA_Config_Type 表规范枚举值 20121119
        OpenAPI = 5,
        Mobile = 6
    }
}
