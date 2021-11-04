function isNullOrEmpty(str) {
    return (!str || str.length === 0 );
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}