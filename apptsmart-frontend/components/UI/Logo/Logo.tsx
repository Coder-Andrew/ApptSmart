interface logoProps {
    className?: string;
    circleColorClass?: string;
    rectColorClass?: string;
};

const Logo = ({
    className,
    circleColorClass = "fill-secondary",
    rectColorClass = "fill-tertiary"
}: logoProps) => {
    

    const cRad = Math.sqrt(880) / 2;
    const cRadOffset = Math.SQRT2 * cRad;
    const cOffset = 1;

    const transforms = [ 0, 90, 180, 270 ];
    return (  
        <>
            <svg
                className={className}
                viewBox={`0 0 100 100`}
                xmlns="http://www.w3.org/2000/svg"
            >
                <rect x={25} y={25} width={50} height={50} className={rectColorClass} />

                { transforms.map(t => (
                    <g key={t} transform={`rotate(${t} 50 50)`}>
                        <circle cx={cRad + cRadOffset - cOffset} cy={cRad} r={cRad} className={circleColorClass} />
                        <circle cx={cRad} cy={cRad + cRadOffset - cOffset} r={cRad} className={circleColorClass} />
                        <rect x={12.5} y={45} width={cRad + 10} height={10} className={circleColorClass} />
                        <circle cx={cRad + cRadOffset - cOffset} cy={cRad + cRadOffset - cOffset} r={cRad} className={circleColorClass} />
                    </g>
                ))}
            </svg>
        </>
    );
}
 
export default Logo;