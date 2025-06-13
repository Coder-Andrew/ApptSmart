import React from "react";

interface TimeSelectProps {
    label: string;
    hour: number;
    minute: number;
    onHourChange: (value: number) => void;
    onMinuteChange: (value: number) => void;
}

const TimeSelect: React.FC<TimeSelectProps> = ({
    label,
    hour,
    minute,
    onHourChange,
    onMinuteChange,
}) => {
    return (
        <div>
            <label>{label}</label>
            <select
                value={hour}
                onChange={(e) => onHourChange(Number(e.target.value))}
            >
                {[...Array(24).keys()].map((i) => (
                    <option key={i}>{i}</option>
                ))}
            </select>
            :
            <select 
                value={minute}
                onChange={(e) => onMinuteChange(Number(e.target.value))}
            >
                { [...Array(60).keys()].map((i) => (
                    <option key={i}>{i}</option>
                ))}
            </select>
        </div>
    );
}
 
export default TimeSelect;