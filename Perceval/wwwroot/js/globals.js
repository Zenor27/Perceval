const cpuUsageHistoric = [];
const ramUsageHistoric = [];

function addUsage(usageList, newUsage) {
    if (usageList.length >= 20) {
        usageList.shift();
    }
    usageList.push(newUsage);
}

function addCpuUsage(newCpuUsage) {
    addUsage(cpuUsageHistoric, newCpuUsage);
}

function addRamUsage(newRamUsage) {
    addUsage(ramUsageHistoric, newRamUsage);
}