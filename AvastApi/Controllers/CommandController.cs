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

		// GET api/values
		[HttpGet(Name = "EpochTime")]
		public ActionResult<double> GetEpochTime()
		{
			try
			{
				var currentEpochTime = this._commandUtility.GetEpochTimeMilliseconds();
				return Ok(currentEpochTime);
			}
			catch(Exception ex)
			{
				this._logger.LogError($"Something went wrong, cannot retrive Current Epoch Time {ex}");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet(Name="NBytes")]
		public ActionResult<byte[]> GetNBytes(string fileName, int length)
		{
			try
			{
				var nbytes = this._commandUtility.GetNBytes(fileName, length);
				return Ok(nbytes);
			}
			catch(Exception ex)
			{
				this._logger.LogError($"Something went wrong, cannot receive from file {fileName} nbytes {ex}");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet(Name="Content")]
		public ActionResult<string> GetContent(string uri)
		{
			try
			{
				var content = this._commandUtility.GetContent(uri);
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