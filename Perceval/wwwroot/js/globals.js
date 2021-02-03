const cpuUsageHistoric = [];

function addCpuUsage(newCpuUsage) {
    if (cpuUsageHistoric.length >= 20) {
        cpuUsageHistoric.shift();
    }
    cpuUsageHistoric.push(newCpuUsage);
}