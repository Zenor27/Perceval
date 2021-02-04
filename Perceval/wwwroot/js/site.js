/*
    Register a plugin that draw a text inside charts
 */
Chart.pluginService.register({
    beforeDraw: function(chart) {
        if (chart.config.options.elements.center) {
            // Get ctx from string
            var ctx = chart.chart.ctx;

            // Get options from the center object in options
            var centerConfig = chart.config.options.elements.center;

            var fontStyle = 'Arial';
            var text = centerConfig.text;

            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';
            var centerX = ((chart.chartArea.left + chart.chartArea.right) / 2);
            var centerY = ((chart.chartArea.top + chart.chartArea.bottom) / 2);
            ctx.font = "normal normal lighter 15px " + fontStyle;


            //Draw text in center
            ctx.fillText(text, centerX, centerY);
        }
    }
});
