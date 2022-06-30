function convertStringToDate(strdate) {
    let dt = new Date(strdate);

    return dt;
}
const countDays = (date_1, date_2) => {
    let difference = date_1.getTime() - date_2.getTime();
    let TotalDays = Math.ceil(difference / (1000 * 3600 * 24));
    return TotalDays;
}