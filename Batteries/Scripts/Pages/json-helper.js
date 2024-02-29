function GetValueForColumnName(columnName, valuesArray, data) {
    var columns = data.columns;
    var index = data.columns.indexOf(columnName);
    if (index >= 0) {
        return valuesArray[index];
    }
    else return NaN;
}

function GetCycleIndexByOrder(idx, data) {
    var cycleIndexArray = data.index;
    return cycleIndexArray[idx];
}