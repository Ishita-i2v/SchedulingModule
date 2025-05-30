﻿
using Microsoft.AspNetCore.Mvc;
using Coravel.Events.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchedulingModule.Managers;
using SchedulingModule.Models;
using SchedulingModule.services;
using Serilog;

namespace SchedulingModule.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    //[LicenceValid]
    public  class SchedulingController : Controller
    {
        
        private readonly ILogger<SchedulingController> _logger;
        public SchedulingController(ILogger<SchedulingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Schedule> GetAll()
        {
            return ScheduleManager.Schedules.Values;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!ScheduleManager.Schedules.ContainsKey(id))
                {
                    return NotFound();
                }
                return Ok(ScheduleManager.Get(id));
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Schedule schedule)
        {
            try
            {
              
              
                ScheduleManager.Add(schedule);
               
            }
            catch (System.Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(schedule);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != schedule.Id)
            {
                return BadRequest();
            }

            try
            {
                ScheduleManager.Update(schedule);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, ex.Message);
                //if (!VideoSourceManager.VideoSources.ContainsKey(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                    throw;
                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(schedule);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!ScheduleManager.Schedules.ContainsKey(id))
            {
                return NotFound();
            }
            try
            {
                var schedule = ScheduleManager.Schedules[id];
                ScheduleManager.Delete(schedule);
                return Ok(schedule);
               
                //return Json(new { status = "error", message = "Schedule Deleted Failed its Atached to a Configuration" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, ex.Message);
                //if (!VideoSourceManager.VideoSources.ContainsKey(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                throw;
                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            //return Ok(schedule);

        }

    }
}
