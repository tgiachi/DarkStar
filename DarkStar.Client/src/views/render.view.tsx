
import {Container, Sprite, Stage, Text, useTick} from "@pixi/react";
import {useReducer, useRef} from "react";


// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
const reducer = (_, { data }) => data;
export const RenderView = () => {


    const Bunny = () => {
        const [motion, update] = useReducer(reducer, null);
        const iter = useRef(0);

        useTick((delta) => {
            const i = (iter.current += 0.05 * delta);


            // eslint-disable-next-line @typescript-eslint/ban-ts-comment
            // @ts-ignore
            update({
                type: 'update',
                data: {
                    x: 300,
                    y: 200,
                    scale: Math.sin(i) * Math.PI,
                    rotation: Math.sin(i) * Math.PI,
                    anchor: Math.sin(i / 2),
                },
            });

        });

        return <Sprite image="https://pixijs.io/pixi-react/img/bunny.png" {...motion} />;
    };
    return (
        <Stage>

            <Container scale={1} >
                 <Bunny />
                <Text text="Hello World"   />
            </Container>
        </Stage>
    )
}

