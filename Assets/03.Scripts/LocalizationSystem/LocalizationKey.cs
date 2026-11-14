using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationKey
{
    /// <summary>是否退出游戏？</summary>
    public const string QuitGameMessage = "是否退出游戏？";
    /// <summary>确定要退出地图工坊吗</summary>
    public const string ExitMapWorkshopMessage = "确定要退出地图工坊吗？";
    /// <summary>确定要清空地图吗</summary>
    public const string ClearMapMessage = "确定要清空地图吗？";
    /// <summary>地图不合法</summary>
    public const string InvalidMap = "地图不合法";
    /// <summary>需要有1个狐狸</summary>
    public const string RequireOneFox = "需要有1个狐狸。";
    /// <summary>最多只能有1个狐狸。</summary>
    public const string CanOnlyHaveOneFox = "最多只能有1个狐狸。";
    /// <summary>至少要有1个宝石。</summary>
    public const string RequireOneGem = "至少要有1个宝石。";
    /// <summary>需要有1个门。</summary>
    public const string RequireOneDoor = "需要有1个门。";
    /// <summary>最多只能有1个门。</summary>
    public const string CanOnlyHaveOneDoor = "最多只能有1个门。";
    /// <summary>名称不能为空</summary>
    public const string NameCannotBeEmpty = "名称不能为空";
    /// <summary>名称不能超过16个字符</summary>
    public const string NameTooLong = "名称不能超过16个字符";
    /// <summary>名称中不能包含以下字符：</summary>
    public const string NameContainsInvalidCharacters = "名称中不能包含以下字符：";
    /// <summary>存在同名地图，保存后将会覆盖</summary>
    public const string DuplicateName = "存在同名地图，保存后将会覆盖";
    /// <summary>名称可用</summary>
    public const string NameAvailable = "名称可用";
    /// <summary>保存成功！</summary>
    public const string SaveSuccess = "保存成功！";
    /// <summary>关卡名</summary>
    public const string LevelName = "第{0}关";
    /// <summary>确定要删除吗？</summary>
    public const string ConfirmDelete = "确定要删除吗？";
    /// <summary>绑定"{0}"</summary>
    public const string Bind = "绑定\"{0}\"";
}
