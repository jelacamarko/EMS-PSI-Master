﻿import React from 'react';
import ReactDOM from 'react-dom';
import HelloWorld from 'HelloWorld.jsx';
import LineChart from 'LineChart.jsx';

const data = [['a', 'b', 'c', 'd', 'e'], [65, 59, 80, 81, 56, 55, 40]];
let InitialLineData = [];

ReactDOM.render(
    <div> <LineChart InitialLineData={data}/><HelloWorld /></div>,
    document.getElementById('Content')
);