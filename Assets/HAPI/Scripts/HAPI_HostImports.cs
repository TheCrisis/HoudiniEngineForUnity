/*
 * PROPRIETARY INFORMATION.  This software is proprietary to
 * Side Effects Software Inc., and is not to be reproduced,
 * transmitted, or disclosed in any way without written permission.
 *
 * Produced by:
 *      Side Effects Software Inc
 *		123 Front Street West, Suite 1401
 *		Toronto, Ontario
 *		Canada   M5J 2M2
 *		416-504-9876
 *
 * COMMENTS:
 * 		Continuation of HAPI_Host class definition. Here we include all libHAPI.dll dll imports.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

// Typedefs (copy these from HAPI_Common.cs)
using HAPI_StringHandle = System.Int32;
using HAPI_AssetLibraryId = System.Int32;
using HAPI_AssetId = System.Int32;
using HAPI_NodeId = System.Int32;
using HAPI_ParmId = System.Int32;
using HAPI_ObjectId = System.Int32;
using HAPI_GeoId = System.Int32;
using HAPI_PartId = System.Int32;
using HAPI_MaterialId = System.Int32;

namespace HAPI 
{	
	/// <summary>
	/// 	Singleton Houdini host object that maintains the singleton Houdini scene and all access to the
	/// 	Houdini runtime.
	/// </summary>
	public static partial class HAPI_Host
	{
#if UNITY_EDITOR
		// INITIALIZATION / CLEANUP ---------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_IsInitialized();

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_Initialize(
			string otl_search_path,
			string dso_search_path,
			HAPI_CookOptions cook_options,
			[ MarshalAs( UnmanagedType.U1 ) ] bool use_cooking_thread,
			int cooking_thread_stack_size );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_Cleanup();

		// DIAGNOSTICS ----------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetEnvInt( HAPI_EnvIntType int_type, out int value );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetStatus( HAPI_StatusType status_code, out int status );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetStatusStringBufLength( HAPI_StatusType status_code, out int buffer_size );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetStatusString( HAPI_StatusType status_code, StringBuilder buffer );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetCookingTotalCount( out int count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetCookingCurrentCount( out int count );

		// UTILITY --------------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ConvertTransform(
			ref HAPI_TransformEuler transform_in_out,
			HAPI_RSTOrder rst_order, HAPI_XYZOrder rot_order );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ConvertMatrixToQuat(
			float[] mat,
			HAPI_RSTOrder rst_order,
			ref HAPI_Transform transform_out );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ConvertMatrixToEuler(
			float[] mat,
			HAPI_RSTOrder rst_order, HAPI_XYZOrder rot_order,
			ref HAPI_TransformEuler transform_out );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ConvertTransformQuatToMatrix(
			HAPI_Transform transform,
			[Out] float[] matrix );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ConvertTransformEulerToMatrix(
			HAPI_TransformEuler transform,
			[Out] float[] matrix );
		
		// STRINGS --------------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetStringBufLength(
			HAPI_StringHandle string_handle,
			out int buffer_length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetString(
			HAPI_StringHandle string_handle,
			StringBuilder string_value,
			int buffer_length );

		// TIME -----------------------------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetTime( out float time );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetTime( float time );

		// ASSETS ---------------------------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_IsAssetValid(
			HAPI_AssetId asset_id, int asset_validation_id,
			out int answer );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result 
		HAPI_LoadAssetLibraryFromFile(
			string file_path,
			out HAPI_AssetLibraryId library_id );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result 
		HAPI_LoadAssetLibraryFromMemory(
			byte[] library_buffer, int library_buffer_size,
			out HAPI_AssetLibraryId library_id );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result 
		HAPI_GetAvailableAssetCount(
			HAPI_AssetLibraryId library_id,
			out int asset_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result 
		HAPI_GetAvailableAssets(
			HAPI_AssetLibraryId library_id,
			[Out] HAPI_StringHandle[] asset_names,
			int asset_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result 
		HAPI_InstantiateAsset(
			string asset_name,
			[ MarshalAs( UnmanagedType.U1 ) ] bool cook_on_load,
			out HAPI_AssetId asset_id );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_CreateCurve( out HAPI_AssetId asset_id );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_CreateInputAsset( out HAPI_AssetId asset_id, string name );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_DestroyAsset( HAPI_AssetId asset_id );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAssetInfo( HAPI_AssetId asset_id, ref HAPI_AssetInfo asset_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_CookAsset(
			HAPI_AssetId asset_id, HAPI_CookOptions cook_options );
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_CookAsset(
			HAPI_AssetId asset_id, System.IntPtr cook_options );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_Interrupt();

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAssetTransform(
			HAPI_AssetId asset_id,
			HAPI_RSTOrder rst_order, HAPI_XYZOrder rot_order,
			out HAPI_TransformEuler transform );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetAssetTransform(
			HAPI_AssetId asset_id,
			ref HAPI_TransformEuler transform );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetInputName( 
			HAPI_AssetId asset_id,
			int input_idx, int input_type,
			out HAPI_StringHandle name );

		// HIP FILES ------------------------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_LoadHIPFile( string file_name, [ MarshalAs( UnmanagedType.U1 ) ] bool cook_on_load );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAssetCountFromLoadHIPFile( ref int asset_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAssetIdsFromLoadHIPFile( [Out] HAPI_AssetId[] asset_ids );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SaveHIPFile( string file_name );

		// NODES ----------------------------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetNodeInfo( HAPI_NodeId node_id, ref HAPI_NodeInfo node_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetGlobalNodes( out HAPI_GlobalNodes global_nodes );

		// PARAMETERS -----------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetParameters(
			HAPI_NodeId node_id,
			[Out] HAPI_ParmInfo[] parm_infos,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetParmIntValues(
			HAPI_NodeId node_id,
			[Out] int[] values,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetParmFloatValues(
			HAPI_NodeId node_id,
			[Out] float[] values,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetParmStringValues(
			HAPI_NodeId node_id,
			[ MarshalAs( UnmanagedType.U1 ) ] bool evaluate,
			[Out] HAPI_StringHandle[] values,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetParmChoiceLists(
			HAPI_NodeId node_id,
			[Out] HAPI_ParmChoiceInfo[] parm_choices, 
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetParmIntValues(
			HAPI_NodeId node_id,
			int[] values,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetParmFloatValues(
			HAPI_NodeId node_id,
			float[] values,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetParmStringValue(
			HAPI_NodeId node_id,
			string value,
			HAPI_ParmId parm_id,
			int index );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_InsertMultiparmInstance(
			HAPI_NodeId node_id,
			HAPI_ParmId parm_id,
			int instance_position );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_RemoveMultiparmInstance(
			HAPI_NodeId node_id,
			HAPI_ParmId parm_id,
			int instance_position );
		
		// HANDLES --------------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetHandleInfo(
			HAPI_AssetId asset_id, 
			[Out] HAPI_HandleInfo[] handle_infos,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetHandleBindingInfo(
			HAPI_AssetId asset_id,
			int handle_index,
			[Out] HAPI_HandleBindingInfo[] handle_infos,
			int start, int length );
		
		// PRESETS --------------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetPresetBufLength( HAPI_NodeId node_id, ref int buffer_length );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetPreset( HAPI_NodeId node_id, [Out] byte[] preset, int buffer_length );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetPreset( HAPI_NodeId node_id, byte[] preset, int buffer_length );
		
		// OBJECTS --------------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetObjects(
			HAPI_AssetId asset_id,
			[Out] HAPI_ObjectInfo[] object_infos, 
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetObjectTransforms(
			HAPI_AssetId asset_id,
			HAPI_RSTOrder rst_order,
			[Out] HAPI_Transform[] transforms,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetInstanceTransforms(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			HAPI_RSTOrder rst_order,
			[Out] HAPI_Transform[] transforms,
			int start, int length );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetObjectTransform(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id,
			HAPI_TransformEuler transform );
		
		// GEOMETRY GETTERS -----------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetGeoInfo(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			out HAPI_GeoInfo geo_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetPartInfo(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			out HAPI_PartInfo part_info );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetFaceCounts(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			[Out] int[] face_counts,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetVertexList(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			[Out] int[] vertex_list,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAttributeInfo(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			string name, HAPI_AttributeOwner owner,
			ref HAPI_AttributeInfo attr_info );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAttributeNames(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			HAPI_AttributeOwner owner,
			[Out] int[] data,
			int count );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAttributeIntData(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			string name,
			ref HAPI_AttributeInfo attr_info,
			[Out] int[] data,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAttributeFloatData(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			string name,
			ref HAPI_AttributeInfo attr_info,
			[Out] float[] data,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetAttributeStrData(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			string name,
			ref HAPI_AttributeInfo attr_info,
			[Out] int[] data,
			int start, int length );
		
		// GEOMETRY SETTERS -----------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetGeoInfo(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			ref HAPI_GeoInfo geo_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetPartInfo(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			ref HAPI_PartInfo part_info );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetFaceCounts(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			int[] face_counts,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetVertexList(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			int[] vertex_list,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_AddAttribute(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			string name,
			ref HAPI_AttributeInfo attr_info );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetAttributeIntData(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			string name,
			ref HAPI_AttributeInfo attr_info,
			int[] data,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetAttributeFloatData(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id,
			string name,
			ref HAPI_AttributeInfo attr_info,
			float[] data,
			int start, int length );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_CommitGeo( HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_RevertGeo( HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id );
		
		// GEOMETRY INPUT -------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetFileInput( HAPI_AssetId asset_id, int input_idx, string file_name );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_CreateGeoInput( 
			HAPI_AssetId asset_id, int input_idx,
			out HAPI_GeoInputInfo geo_input_info );
		
		// INTER ASSET ----------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ConnectAssetTransform(
			HAPI_AssetId asset_id_from, HAPI_AssetId asset_id_to, int input_idx );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_DisconnectAssetTransform( HAPI_AssetId asset_id, int input_idx );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ConnectAssetGeometry( 
			HAPI_AssetId asset_id_from, HAPI_ObjectId object_id_from,
			HAPI_AssetId asset_id_to,
			int input_idx );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_DisconnectAssetGeometry( HAPI_AssetId asset_id, int input_idx );
		
		// MATERIALS ------------------------------------------------------------------------------------------------
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetMaterial(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			out HAPI_MaterialInfo material_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_RenderMaterialToImage(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			HAPI_ShaderType shader_type );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_RenderTextureToImage(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			HAPI_ParmId parm_id );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetSupportedImageFileFormatCount( out int file_format_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetSupportedImageFileFormats(
			[Out] HAPI_ImageFileFormat[] formats,
			int file_format_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetImageInfo(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			out HAPI_ImageInfo image_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetImageInfo(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			HAPI_ImageInfo image_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetImagePlaneCount(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			out int image_plane_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetImagePlanes(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			[Out] HAPI_StringHandle[] image_planes,
			int image_plane_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ExtractImageToFile(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			string image_file_format_name,
			string image_planes,
			string destination_folder_path,
			string destination_file_name,
			out int destination_file_path );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ExtractImageToMemory(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			string image_file_format_name,
			string image_planes,
			out int buffer_size );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetImageMemoryBuffer(
			HAPI_AssetId asset_id, HAPI_MaterialId material_id,
			[Out] byte[] buffer,
			int buffer_size );

		// SIMULATION/ANIMATIONS ------------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetAnimCurve(
			HAPI_NodeId node_id, HAPI_ParmId parm_id,
			HAPI_Keyframe[] curve_keyframes,
			int keyframe_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_SetTransformAnimCurve(
			HAPI_NodeId node_id, int transform_component,
			HAPI_Keyframe[] curve_keyframes,
			int keyframe_count );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_ResetSimulation( HAPI_AssetId asset_id );

		// VOLUMES --------------------------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetVolumeInfo(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			ref HAPI_VolumeInfo volume_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetFirstVolumeTile(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			ref HAPI_VolumeTileInfo tile );
		
		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetNextVolumeTile(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			ref HAPI_VolumeTileInfo next );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetVolumeTileFloatData(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			ref HAPI_VolumeTileInfo tile,
			[Out] float[] values );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetVolumeTileIntData(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			ref HAPI_VolumeTileInfo tile,
			[Out] int[] values );

		// CURVES ---------------------------------------------------------------------------------------------------

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetCurveInfo(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			ref HAPI_CurveInfo curve_info );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetCurveVertices(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			[Out] float[] vertices,
			int start, int length );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetCurveCounts(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			[Out] int[] counts,
			int start, int length );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetCurveOrders(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			[Out] int[] orders,
			int start, int length );

		[ DllImport( "libHAPI", CallingConvention = CallingConvention.Cdecl ) ]
		private static extern HAPI_Result
		HAPI_GetCurveKnots(
			HAPI_AssetId asset_id, HAPI_ObjectId object_id, HAPI_GeoId geo_id, HAPI_PartId part_id,
			[Out] float[] knots,
			int start, int length );

#endif // UNITY_EDITOR
	}

}
