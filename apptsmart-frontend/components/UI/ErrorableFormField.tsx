import React from "react";

interface ErrorableFormFieldProps {
    id: string,
    label: string;
    type?: string;
    value: string|number;
    placeholder?: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    error?: string;

    classNameWrapper?: string;
    classNameLabel?: string;
    classNameInput?: string;
    classNameError?: string;
}

const ErrorableFormField: React.FC<ErrorableFormFieldProps> = ({
    id,
    label,
    type = "text",
    value,
    placeholder,
    onChange,
    error,
    classNameWrapper = "",
    classNameLabel = "",
    classNameInput = "",
    classNameError = "",
}) => {

    return (
        <div className={classNameWrapper}>
            {error && <p className={classNameError}>{error}</p>}
            <label htmlFor={id} className={classNameLabel}>
                {label}
            </label>
            <input 
                id={id}
                type={type}
                value={value}
                placeholder={placeholder}
                onChange={onChange}
                className={classNameInput}
            />
        </div>
    );
}
 
export default ErrorableFormField;