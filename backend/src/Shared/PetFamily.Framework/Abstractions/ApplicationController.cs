using Microsoft.AspNetCore.Mvc;

namespace PetFamily.Framework.Abstractions;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase
{
}