namespace Yintai.Hangzhou.Contract.Images
{
	/// <summary>
	/// 图片上传返回枚举
	/// </summary>
	public enum FileMessage
	{
		/// <summary>
		/// 文件类型错误
		/// </summary>
		ExtError,

		/// <summary>
		/// 文件超出设定大小
		/// </summary>
		SizeError,
		
		/// <summary>
		/// 未知错误
		/// </summary>
		UnknowError,
		
		/// <summary>
		/// 上传成功
		/// </summary>
		Success
	}
}
