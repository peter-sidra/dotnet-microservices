using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos {
    public class CommandCreateDto {
        // We're not including the platform id in the dto because
        // it will be specified in the route and not in the payload body

        [Required]
        public string HowTo { get; set; }
        [Required]
        public string CommandLine { get; set; }
    }
}