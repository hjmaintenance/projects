window.activatePanel = function (panelId) {
    var allPanels = document.querySelectorAll('.side-Menu-pannel');
    allPanels.forEach(function (panel) {
        panel.style.display = 'none';
    });

    var selectedPanel = document.getElementById(panelId);
    if (selectedPanel) {
        selectedPanel.style.display = 'flex';
    }
};
