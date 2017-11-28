using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos.QueryDtos;
using szakdoga.Others;

namespace szakdoga.Controllers
{
    [Authorize]
    [Route("api/queries")]
    public class QueryController : Controller
    {
        private readonly QueryManager _manager;
        private readonly ILogger<QueryController> _logger;

        public QueryController(QueryManager manager, ILogger<QueryController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public IActionResult GetQueries()
        {
            try
            {
                var queries = _manager.GetAll();
                return Ok(queries);
            }
            catch (BasicException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("GetQueryColumns/{queryGUID}")]
        public IActionResult GetQueryColumns(string queryGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(queryGUID)) throw new BasicException("Empty queryGUID.");
                var result = _manager.GetColumnNames(queryGUID);
                return Ok(result);
            }
            catch (BasicException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("GetQuerySource")]
        public IActionResult GetQuerySource([FromBody] QuerySourceFilterDto filter)
        {
            try
            {
                if (filter == null) throw new BasicException("Wrong input format.");
                if (!ModelState.IsValid)
                    BadRequest(ModelState);

                var jsonResult = _manager.GetQuerySource(filter);

                return Ok(jsonResult);
            }
            catch (BasicException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("GetQueryColumnCount/{queryGUID}")]
        public IActionResult GetQueryColumnCount(string queryGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(queryGUID)) throw new BasicException("Empty queryGUID");

                return Ok(_manager.GetQueryColumnCount(queryGUID));
            }
            catch (BasicException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}