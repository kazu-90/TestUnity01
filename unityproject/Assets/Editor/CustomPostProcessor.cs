/*!
 * ビルド時に行う追加処理
 * 
 * @file	CustomPostProcessor.cs
 * @author	k-fujiya
 * @date	2017/11/09
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;

public static class CustomPostProcessor {
	private const string URL_SCHEME = "testScheme";		// URLスキーム
	private const string DEVELOPMENT_REGION = "ja_JP";	// リージョン



	[PostProcessBuild(1000)]
	public static void OnPostProcessBuild(BuildTarget target, string path) {
		Debug.unityLogger.Log("OnPostProcessBuild");
		if(target == BuildTarget.iOS){
			PostProcessBuild_iOS(path);
		}
		else if(target == BuildTarget.Android){
			PostProcessBuild_Android(path);
		}
	}


#region IOS
	/// <summary>
	/// iOS側のビルド処理を行う
	/// </summary>
	/// <param name="path">Path.</param>
	private static void PostProcessBuild_iOS(string path) {
		Debug.unityLogger.Log("PostProcessBuild_iOS");

		// PBXProjectの初期化
		var projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
		PBXProject pbxProject = new PBXProject();
		pbxProject.ReadFromFile(projectPath);
		string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

		CreateXProjectFile(pbxProject, targetGuid, path);

		// 設定を反映
		pbxProject.WriteToFile(projectPath);
		//File.WriteAllText(projectPath, pbxProject.WriteToString());

		//Info.plist を書き換える
		ReWriteInfoPList(path);
	}

	/// <summary>
	/// CreateXProjectFile ファイルの作成
	/// </summary>
	/// <param name="pbxProject">プロジェクトクラス</param>
	/// <param name="targetGuid">XcodeプロジェクトのメインターゲットのGUID</param>
	/// <param name="path">Path.</param>
	private static void CreateXProjectFile(PBXProject pbxProject, string targetGuid, string path) {
		// ビルド設定の追加、変更
		AddBuildSetting(pbxProject, targetGuid);

		// フレームワークを追加
		AddFramework(pbxProject, targetGuid);

		// ファイルのフラグ更新
		SetCompileFlags(pbxProject, targetGuid, path);

		// Capabilityの設定
		AddCapability(pbxProject, targetGuid);
	}

	/// <summary>
	/// ビルド設定の追加、変更
	/// </summary>
	/// <param name="pbxProject">プロジェクトクラス</param>
	/// <param name="targetGuid">XcodeプロジェクトのメインターゲットのGUID</param>
	private static void AddBuildSetting(PBXProject pbxProject, string targetGuid) {
		// ビルド設定の追加
		//pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-all_load");

		// ビルド設定の追加
		pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");

		// ビルド設定の編集、第3引数は追加する設定、第4引数は削除する設定
		List<string> otherLinkerFlags = new List<string>();
		otherLinkerFlags.Add("-ObjC");
		pbxProject.UpdateBuildProperty(targetGuid, "OTHER_LDFLAGS", otherLinkerFlags, new string[] { });
	}

	/// <summary>
	/// フレームワークの追加
	/// </summary>
	/// <param name="pbxProject">プロジェクトクラス</param>
	/// <param name="targetGuid">XcodeプロジェクトのメインターゲットのGUID</param>
	private static void AddFramework(PBXProject pbxProject, string targetGuid){
		// 必須フレームワークの追加（昔の例：Security.framework）
		pbxProject.AddFrameworkToProject(targetGuid, "Security.framework", false);
		pbxProject.AddFrameworkToProject(targetGuid, "iAd.framework", false);

		// オプションフレームワークの追加（昔の例：WebKit.framework:weak）
		pbxProject.AddFrameworkToProject(targetGuid, "SafariServices.framework", true);
		pbxProject.AddFrameworkToProject(targetGuid, "WebKit.framework", true);
	}

	/// <summary>
	/// コンパイルフラグの設定
	/// </summary>
	/// <param name="pbxProject">プロジェクトクラス</param>
	/// <param name="targetGuid">XcodeプロジェクトのメインターゲットのGUID</param>
	/// <param name="path">プロジェクトパス</param>
	private static void SetCompileFlags(PBXProject pbxProject, string targetGuid, string path) {
#if UNITY_5_3_OR_NEWER
		// To need to add a `-fno-objc-arc` flag if building in Unity5.
		// 格納されたプラグインのファイルの一覧を取得
		string librariesPath = System.IO.Path.Combine(path, "Libraries/Plugins/iOS");
		string[] filePathList = System.IO.Directory.GetFiles(librariesPath);

		foreach(string targetPath in filePathList) {
			string extention = System.IO.Path.GetExtension(targetPath);
			// 指定された拡張子以外は処理を行わない。
			if(!(extention == ".m" || extention == ".mm")) {
				continue;
			}

			// パスの内容を修正後、guidの取得を行う
			string findPath = targetPath.Replace(path + "/", "");
			Debug.Log("findPath : " + findPath);
			var guid = pbxProject.FindFileGuidByProjectPath(findPath);

			if(guid == null) {
				Debug.Log("guid NULL");
				continue;
			}
			var flags = pbxProject.GetCompileFlagsForFile(targetGuid, guid);
			if(flags == null) {
				Debug.Log("flags NULL");
				continue;
			}
			flags.Add("-fno-objc-arc");// フラグを変換する
			pbxProject.SetCompileFlagsForFile(targetGuid, guid, flags);

		}
#endif
	}

	/// <summary>
	/// Capabilityの設定
	/// </summary>
	/// <param name="pbxProject">プロジェクトクラス</param>
	/// <param name="targetGuid">XcodeプロジェクトのメインターゲットのGUID</param>
	private static void AddCapability(PBXProject pbxProject, string targetGuid) {

		//pbxProject.AddCapability(targetGuid, PBXCapabilityType.PushNotifications); //プッシュ通知機能をONにする
	}








	/// <summary>
	/// Info.plistを書き換える
	/// </summary>
	/// <param name="path">Path.</param>
	private static void ReWriteInfoPList(string path) {
		/* サンプル
		// Plistの設定のための初期化
		var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
		var plist = new PlistDocument();
		plist.ReadFromFile(plistPath);

		// 文字列の設定
		plist.root.SetString("hogehogeId", "dummyid"); 		*/

		try {
			// plistがあるかチェック
			string file = System.IO.Path.Combine(path, "Info.plist");
			if(!File.Exists(file)) {
				return;
			}

			// Plistの設定のための初期化
			string plistPath = Path.Combine(path, "Info.plist");
			PlistDocument plist = new PlistDocument();
			plist.ReadFromFile(plistPath);

			// URLスキームを設定
			AddUrlScheme(plist, URL_SCHEME);

			// リージョン設定
			AddDevelopmentRegion(plist, DEVELOPMENT_REGION);

			// 設定を保存
			plist.WriteToFile(plistPath);

		}catch(Exception e){
			Debug.Log("Unable to update Info.plist: " + e);
		}
	}


	/// <summary>
	/// Info.plist に URL Scheme を設定する
	/// </summary>
	/// <param name="plist">Plist.</param>
	/// <param name="urlScheme">URL scheme.</param>
	private static void AddUrlScheme(PlistDocument plist, string urlScheme) {
		PlistElementArray array = plist.root.CreateArray("CFBundleURLTypes");
		var urlDict = array.AddDict();
		urlDict.SetString("CFBundleURLName", "com.xxx." + urlScheme);
		var urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
		urlInnerArray.AddString(urlScheme);
	}

	/// <summary>
	/// Info.plist にリージョンを設定する
	/// </summary>
	/// <param name="plist">plist</param>
	/// <param name="region">Region.</param>
	private static void AddDevelopmentRegion(PlistDocument plist, string region) {
		plist.root.SetString("CFBundleDevelopmentRegion", region);
	}

#endregion IOS


	private static void PostProcessBuild_Android(string path) {
	}

}
