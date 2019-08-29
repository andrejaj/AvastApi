using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Net;
using AvastApi.Utility;
using Microsoft.Extensions.Logging;

namespace AvastApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommandController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly ICommandUtility _commandUtility;
		public CommandController(ILogger<CommandController> logger, ICommandUtility commandUtility)
		{
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this._commandUtility = commandUtility ?? throw new ArgumentNullException(nameof(commandUtility));
		}

		/// <summary>
		/// Get current time in milliseconds
		/// </summary>
		/// <returns>double</returns>
		// GET api/values
		[HttpGet("timenow")]
		[ProducesResponseType(typeof(double), 200)]
		[ProducesResponseType(500)]
		public ActionResult<double> GetTimeNowInMilliseconds()
		{
			try
			{
				var timeNow = this._commandUtility.GetTimeNowInMilliseconds();
				return Ok(timeNow);
			}
			catch (Exception ex)
			{
				this._logger.LogError($"Something went wrong, cannot retrive Current Epoch Time {ex}");
				return StatusCode(500, "Internal server error");
			}
		}

		/// <summary>
		/// Get first n bytes from a 'DemoFile.txt' file.
		/// </summary>
		/// <param name="length">Length of bytes to take from a file.</param>
		/// <returns>byte[]</returns>
		[HttpGet("nbytes/{length}")]
		[ProducesResponseType(typeof(byte[]), 200)]
		[ProducesResponseType(500)]
		public ActionResult<byte[]> GetNBytes(int length)
		{
			try
			{
				var nbytes = this._commandUtility.GetNBytes("DemoFile.txt", length);
				return Ok(nbytes);
			}
			catch(Exception ex)
			{
				this._logger.LogError($"Something went wrong, cannot receive nbytes from a file. {ex}");
				return StatusCode(500, "Internal server error");
			}
		}

		/// <summary>
		/// Get content of a URL.
		/// </summary>
		/// <param name="uri">Uri to fetch content from.</param>
		/// <returns>string</returns>
		[HttpGet("content/{uri}")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(500)]
		public ActionResult<string> GetContent(string uri)
		{
			try
			{
				var url = $"http://{uri}";
				var content = this._commandUtility.GetContent(url);
				return Ok(content);
			}
			catch(Exception ex)
			{
				this._logger.LogError($"Something went wrong, cannot receive {uri} file content {ex}");
				return StatusCode(500, "Internal server error");
			}
		}
	}
}