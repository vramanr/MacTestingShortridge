using CalibrationManagement.Core.Entities;
using CalibrationManagement.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CalibrationManagement.Application.Services
{
    public interface IMultiModeCalibrationService
    {
        Task<List<CalSetup>> GetActiveModesForCalibrationTypeAsync(string calType);
        Task<List<CalSetup>> GetAvailableModesAsync(string calType);
        Task<bool> ValidateModeCompatibilityAsync(string calType, List<string> selectedModes);
        Task<bool> ValidateModeSelectionAsync(string calType, List<string> selectedModes);
        Task<CalSetup?> GetModeConfigurationAsync(string calType, string mode);
        Task<List<CalStandards>> GetStandardsForModeAsync(string mode, string standardType);
        Task<bool> IsFlowVelocityConflictAsync(string calType, List<string> selectedModes);
        Task<Dictionary<string, object>> GetModeSpecificValidationRulesAsync(string mode);
        Task<bool> ValidateTemProbeConfigurationAsync(string calType, string mode);
        bool AreModesCompatible(List<string> selectedModes);
    }

    public class MultiModeCalibrationService : IMultiModeCalibrationService
    {
        private readonly ILogger<MultiModeCalibrationService> _logger;
        private readonly CalibrationDbContext _context;

        public MultiModeCalibrationService(
            ILogger<MultiModeCalibrationService> logger,
            CalibrationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<CalSetup>> GetActiveModesForCalibrationTypeAsync(string calType)
        {
            try
            {
                var modes = await _context.CalSetup
                    .Where(cs => cs.CalType == calType && cs.Active == true)
                    .OrderBy(cs => cs.SortOrder)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} active modes for calibration type {CalType}", modes.Count, calType);
                return modes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active modes for calibration type {CalType}", calType);
                throw;
            }
        }

        public async Task<bool> ValidateModeCompatibilityAsync(string calType, List<string> selectedModes)
        {
            try
            {
                var hasFlowEqv = selectedModes.Any(m => m.Contains("Flow Eqv"));
                var hasVelocityEqv = selectedModes.Any(m => m.Contains("Velocity Eqv"));

                if (hasFlowEqv && hasVelocityEqv)
                {
                    _logger.LogWarning("Flow Eqv and Velocity Eqv modes cannot be selected simultaneously");
                    return false;
                }

                var availableModes = await GetActiveModesForCalibrationTypeAsync(calType);
                var availableModeNames = availableModes.Select(m => m.Mode).ToList();

                foreach (var selectedMode in selectedModes)
                {
                    if (!availableModeNames.Contains(selectedMode))
                    {
                        _logger.LogWarning("Mode {Mode} is not available for calibration type {CalType}", selectedMode, calType);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating mode compatibility for calibration type {CalType}", calType);
                return false;
            }
        }

        public async Task<CalSetup?> GetModeConfigurationAsync(string calType, string mode)
        {
            try
            {
                var config = await _context.CalSetup
                    .FirstOrDefaultAsync(cs => cs.CalType == calType && cs.Mode == mode && cs.Active == true);

                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving mode configuration for {CalType}/{Mode}", calType, mode);
                throw;
            }
        }

        public async Task<List<CalStandards>> GetStandardsForModeAsync(string mode, string standardType)
        {
            try
            {
                var modeConfig = await _context.CalSetup
                    .FirstOrDefaultAsync(cs => cs.Mode == mode && cs.Active == true);

                if (modeConfig == null)
                    return new List<CalStandards>();

                var standardField = standardType == "primary" ? modeConfig.CalStd : modeConfig.CalStd2;

                if (string.IsNullOrEmpty(standardField))
                    return new List<CalStandards>();

                var standards = await _context.CalStandards
                    .Where(cs => cs.Name != null)
                    .OrderBy(cs => cs.Name)
                    .ToListAsync();

                
                return standards;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving standards for mode {Mode}, type {StandardType}", mode, standardType);
                throw;
            }
        }

        public async Task<bool> IsFlowVelocityConflictAsync(string calType, List<string> selectedModes)
        {
            try
            {
                var hasFlowEqv = selectedModes.Any(m => m == "Flow Eqv");
                var hasVelocityEqv = selectedModes.Any(m => m == "Velocity Eqv");

                if (hasFlowEqv || hasVelocityEqv)
                {
                    return hasFlowEqv && hasVelocityEqv;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking flow/velocity conflicts for calibration type {CalType}", calType);
                return true; // Err on the side of caution
            }
        }

        public async Task<Dictionary<string, object>> GetModeSpecificValidationRulesAsync(string mode)
        {
            try
            {
                var rules = new Dictionary<string, object>();

                switch (mode)
                {
                    case "TemProbe Temp":
                        rules["requiresSecondaryStandard"] = true;
                        rules["columnVisibility"] = new { Column3 = true };
                        break;

                    case "Flow Eqv":
                        rules["conflictsWith"] = new[] { "Velocity Eqv" };
                        rules["requiresFlowStandards"] = true;
                        break;

                    case "Velocity Eqv":
                        rules["conflictsWith"] = new[] { "Flow Eqv" };
                        rules["requiresVelocityStandards"] = true;
                        break;

                    case "Diff Pressure":
                        rules["requiresPressureStandards"] = true;
                        rules["standardType"] = "adm_diffpr";
                        break;

                    case "Abs Pressure":
                        rules["requiresPressureStandards"] = true;
                        rules["standardType"] = "adm_absprs";
                        break;

                    case "Meter Temp":
                        rules["requiresTemperatureStandards"] = true;
                        rules["standardType"] = "met_temp";
                        break;

                    default:
                        rules["standardType"] = mode.ToLower().Replace(" ", "_");
                        break;
                }

                return rules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving validation rules for mode {Mode}", mode);
                throw;
            }
        }

        public async Task<bool> ValidateTemProbeConfigurationAsync(string calType, string mode)
        {
            try
            {
                if (mode != "TemProbe Temp")
                    return true;

                var modeConfig = await GetModeConfigurationAsync(calType, mode);
                
                if (modeConfig == null)
                    return false;

                var hasPrimaryStandard = !string.IsNullOrEmpty(modeConfig.CalStd);
                var hasSecondaryStandard = !string.IsNullOrEmpty(modeConfig.CalStd2);

                return hasPrimaryStandard && hasSecondaryStandard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating TemProbe configuration for {CalType}/{Mode}", calType, mode);
                return false;
            }
        }

        public async Task<List<CalSetup>> GetAvailableModesAsync(string calType)
        {
            return await GetActiveModesForCalibrationTypeAsync(calType);
        }

        public async Task<bool> ValidateModeSelectionAsync(string calType, List<string> selectedModes)
        {
            return await ValidateModeCompatibilityAsync(calType, selectedModes);
        }

        public bool AreModesCompatible(List<string> selectedModes)
        {
            if (selectedModes == null || selectedModes.Count == 0)
                return true;

            var hasFlowEqv = selectedModes.Any(m => m.Contains("Flow Eqv"));
            var hasVelocityEqv = selectedModes.Any(m => m.Contains("Velocity Eqv"));

            if (hasFlowEqv && hasVelocityEqv)
                return false;

            return true;
        }
    }
}
